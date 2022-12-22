namespace Namespace {
    
    using itertools;
    
    using rd = random;
    
    using time;
    
    using np = numpy;
    
    using pseudoStateObserver = pseudoStateObserver.pseudoStateObserver;
    
    using Actor = CodeFramework.Actor.Actor;
    
    using System.Collections.Generic;
    
    using System.Linq;
    
    using System;
    
    using PoppyTorso = poppy.creatures.PoppyTorso;
    
    using np = numpy;
    
    using time;
    
    using math;
    
    using GridStateActionSpace2D = CodeFramework.GridStateActionSpace.GridStateActionSpace2D;
    
    public static class Module {
        
        public static object @__author__ = "ben";
        
        // Agent in the trainning enviromtn
        public class actorVrep
            : pseudoStateObserver, Actor {
            
            public actorVrep(object poppy, object io, object name, object positionMatrix)
                : base(io, name, positionMatrix) {
                this.positionMatrix = positionMatrix;
                this.poppy = poppy;
                this.io = io;
                this.name = name;
            }
            
            //  Motor control interface and unexpected to be called outside the class
            //             action: Head moves left: (-1, 0), right:(0, 1), rightdown(1, -1) etc.
            //             motionUnit: used to accelerate or slow down the movement
            //             
            public virtual object @__motorControl(object action, object motionUnit) {
                var _tup_1 = action;
                var m = _tup_1.Item1;
                var n = _tup_1.Item2;
                var angleY = this.poppy.head_y.present_position;
                var angleZ = this.poppy.head_z.present_position;
                if (m != 0 && n != 0) {
                    m = m / abs(m);
                    n = n / abs(n);
                    this.poppy.head_z.goal_position = angleZ + 1.5 * motionUnit * m;
                    this.poppy.head_y.goal_position = angleY + 1 * motionUnit * n;
                }
                if (m != 0 && n == 0) {
                    m = m / abs(m);
                    this.poppy.head_z.goal_position = angleZ + 1.5 * motionUnit * m;
                }
                if (m == 0 && n != 0) {
                    n = n / abs(n);
                    this.poppy.head_y.goal_position = angleY + 1 * motionUnit * n;
                }
                time.sleep(0.3);
            }
            
            //  This function performs the action using the function motorControl 
            public virtual object perform_action(object action) {
                var motionUnit = 1;
                this.@__motorControl(action, motionUnit);
                return true;
            }
            
            //  Initialises a new episode i.e. sets the cube to a new random position 
            public virtual object initialise_episode() {
                this.poppy.head_z.goal_position = 0;
                this.poppy.head_y.goal_position = rd.choice(new List<object> {
                    -5,
                    -4,
                    -3,
                    -2,
                    -1,
                    0,
                    1,
                    2,
                    3,
                    4
                });
                var horizontal_pos = new List<object> {
                    -0.6,
                    -0.4,
                    -0.2,
                    0,
                    0.2,
                    0.4,
                    0.6
                };
                var cube_position_old = this.io.get_object_position("cube");
                this.io.set_object_position("cube", position: new List<object> {
                    rd.choice(horizontal_pos),
                    -1,
                    1.05
                });
                time.sleep(0.5);
                var cube_position_new = this.io.get_object_position("cube");
                var currentState = ((actorVrep) this).get_current_state();
                // The while loop ensures that the cube can be seen at the new position and is not in the same position as before
                while (currentState == new List<object>() || cube_position_old == cube_position_new) {
                    this.io.set_object_position("cube", position: new List<object> {
                        rd.choice(horizontal_pos),
                        -1,
                        1.05
                    });
                    time.sleep(0.5);
                    cube_position_new = this.io.get_object_position("cube");
                    currentState = ((actorVrep) this).get_current_state();
                }
                return true;
            }
            
            //  Use closed loop control to move the agent to the next corresponding state.
            //             For definition of action see self.__motorControl.
            //             When no obect in sight, return False. 
            public virtual object @__perform_action_old(object action) {
                // this function is no longer used
                if (action == (0, 0)) {
                    return "action illegal";
                }
                var currentState = ((actorVrep) this).get_current_state();
                if (currentState.ToList().Count == 0) {
                    return false;
                }
                var _tup_1 = currentState;
                var x = _tup_1.Item1;
                var y = _tup_1.Item2;
                var _tup_2 = action;
                var diffX = _tup_2.Item1;
                var diffY = _tup_2.Item2;
                var goalX = x + diffX;
                var goalY = y + diffY;
                var count = 0;
                while ((x != goalX || y != goalY) && count < 20) {
                    var actionX = goalX - x;
                    var actionY = goalY - y;
                    var a = max(abs(actionX) * 1.5 / 5, 1);
                    var b = max(abs(actionY) / 5, 1);
                    var motionUnit = min(a, b);
                    this.@__motorControl((actionX, actionY), motionUnit);
                    currentState = ((actorVrep) this).get_current_state();
                    if (currentState.ToList().Count == 0) {
                        return false;
                    }
                    var _tup_3 = currentState;
                    x = _tup_3.Item1;
                    y = _tup_3.Item2;
                    count += 1;
                }
                return true;
            }
            
            //  Agent moves to a random state by being given state space 
            public virtual object @__randMove_old(object stateSpace) {
                // this function is no longer used
                while (((actorVrep) this).get_current_state().ToList().Count == 0) {
                    var list1 = np.arange(-this.positionMatrix[0], this.positionMatrix[0]);
                    var list2 = np.arange(-this.positionMatrix[1], this.positionMatrix[1]);
                    var motionSpace = (from _tup_1 in itertools.product(list1, list2).Chop((i,j) => (i, j))
                        let i = _tup_1.Item1
                        let j = _tup_1.Item2
                        select (i, j)).ToList();
                    motionSpace.remove((0, 0));
                    this.perform_action(motionSpace[rd.randint(0, motionSpace.Count - 1)]);
                }
                var initialState = stateSpace[rd.randint(0, stateSpace.Count - 1)];
                while (initialState == (0, 0)) {
                    initialState = stateSpace[rd.randint(0, stateSpace.Count - 1)];
                }
                var _tup_2 = ((actorVrep) this).get_current_state();
                var x = _tup_2.Item1;
                var y = _tup_2.Item2;
                var _tup_3 = initialState;
                var initialX = _tup_3.Item1;
                var initialY = _tup_3.Item2;
                var diffX = Convert.ToInt32(initialX - x);
                var diffY = Convert.ToInt32(initialY - y);
                while (!this.perform_action((diffX, diffY))) {
                    initialState = stateSpace[rd.randint(0, stateSpace.Count - 1)];
                    while (initialState == (0, 0)) {
                        initialState = stateSpace[rd.randint(0, stateSpace.Count - 1)];
                    }
                    var _tup_4 = ((actorVrep) this).get_current_state();
                    x = _tup_4.Item1;
                    y = _tup_4.Item2;
                    var _tup_5 = initialState;
                    initialX = _tup_5.Item1;
                    initialY = _tup_5.Item2;
                    diffX = Convert.ToInt32(initialX - x);
                    diffY = Convert.ToInt32(initialY - y);
                    this.perform_action((diffX, diffY));
                }
            }
        }
        
        static Module() {
            @" Only for testing - no modification necessary ";
            io.add_cube(name, position, sizes, mass);
            time.sleep(1);
            io.add_cube(name1, position1, sizes1, mass);
            io.set_object_position("cube", position: new List<object> {
                0,
                -1,
                1.05
            });
            @"
    for i in xrange(10):
        a.initialise_episode()
        print 'New Position cube at: ', io.get_object_position('cube')
    ";
            a.perform_action((0, 1));
            time.sleep(1);
            a.perform_action((0, -1));
            time.sleep(1);
            a.perform_action((1, 1));
            time.sleep(1);
            a.perform_action((1, -1));
            time.sleep(1);
            a.perform_action((1, 0));
            time.sleep(1);
            a.perform_action((-1, 0));
            time.sleep(1);
            a.perform_action((-1, 1));
            time.sleep(1);
            a.perform_action((-1, -1));
            time.sleep(1);
        }
        
        public static object poppy = PoppyTorso(simulator: "vrep");
        
        public static object io = poppy._controllers[0].io;
        
        public static object name = "cube";
        
        public static object position = new List<object> {
            0,
            -0.15,
            0.85
        };
        
        public static object sizes = new List<object> {
            0.1,
            0.1,
            0.1
        };
        
        public static object mass = 0;
        
        public static object name1 = "cube2";
        
        public static object position1 = new List<object> {
            0,
            -1,
            0.5
        };
        
        public static object sizes1 = new List<object> {
            3,
            1,
            1
        };
        
        public static object positionMatrix = (25, 20);
        
        public static object a = actorVrep(poppy, io, name, positionMatrix);
        
        public static object b = GridStateActionSpace2D(dimensions: positionMatrix, allow_diag_actions: true);
        
        public static object c = pseudoStateObserver(poppy, io, name, positionMatrix);
    }
}
