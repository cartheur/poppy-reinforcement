namespace Namespace {
    
    using itertools;
    
    using rd = random;
    
    using time;
    
    using np = numpy;
    
    using Actor = CodeFramework.Actor.Actor;
    
    using System.Collections.Generic;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        //  Real Poppy 
        public class actorPoppy
            : Actor {
            
            public actorPoppy(object poppy, object dimensions = (3, 1)) {
                this.dimensions = dimensions;
                this.poppy = poppy;
                var ids = new List<object> {
                    36,
                    37
                };
                var speed = zip(ids, itertools.repeat(200)).ToDictionary();
                this.poppy.enable_torque(ids);
                this.poppy.set_moving_speed(speed);
            }
            
            //  Action may be not deterministic and may not change the state
            // 			Params:
            // 				action: tuple(m, n) next trend state (x + m, y + n)
            // 			
            public virtual object perform_action(object action) {
                object goalY;
                object goalZ;
                var _tup_1 = action;
                var m = _tup_1.Item1;
                var n = _tup_1.Item2;
                if (m == 0 && n == 0) {
                    return false;
                }
                if (m == 0) {
                    n = abs(n) / n;
                }
                if (n == 0) {
                    m = abs(m) / m;
                }
                var angleY = this.poppy.get_present_position(ValueTuple.Create(37))[0];
                var angleZ = this.poppy.get_present_position(ValueTuple.Create(36))[0];
                var motionUnit = 3;
                if (m != 0 && n != 0) {
                    goalZ = angleZ + 1.5 * motionUnit * m;
                    goalY = angleY - 1 * motionUnit * n;
                    var pos = zip(new List<object> {
                        36,
                        37
                    }, new List<object> {
                        goalZ,
                        goalY
                    }).ToDictionary();
                    this.poppy.set_goal_position(pos);
                }
                if (m != 0 && n == 0) {
                    goalZ = angleZ + 1.5 * motionUnit * m;
                    this.poppy.set_goal_position(new Dictionary<object, object> {
                        {
                            36,
                            goalZ}});
                }
                if (m == 0 && n != 0) {
                    goalY = angleY - 1 * motionUnit * n;
                    this.poppy.set_goal_position(new Dictionary<object, object> {
                        {
                            37,
                            goalY}});
                }
                time.sleep(0.04);
            }
            
            public virtual object come_to_zero() {
                var pos = zip(new List<object> {
                    36,
                    37
                }, new List<object> {
                    0,
                    -25
                }).ToDictionary();
                this.poppy.set_goal_position(pos);
                time.sleep(0.04);
            }
            
            //  Initialize by changing the position of red point 
            public virtual object initialise_episode(object stateSpace) {
                this.come_to_zero();
            }
        }
        
        static Module() {
            @" Only for testing and need not to be modified ";
        }
    }
}
