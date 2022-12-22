namespace Namespace {
    
    using StateObserver = CodeFramework.StateObserver.StateObserver;
    
    using cvVrep = CVVrep.cvVrep;
    
    using PoppyTorso = poppy.creatures.PoppyTorso;
    
    using np = numpy;
    
    using time;
    
    using math;
    
    using System.Collections.Generic;
    
    public static class Module {
        
        //  Use pseudoCV algorithm to observe the agent current state
        public class pseudoStateObserver
            : StateObserver, cvVrep {
            
            public pseudoStateObserver(object poppy, object io, object name, object positionMatrix)
                : base(poppy, io, name, positionMatrix) {
            }
            
            //  Return the current state 
            public virtual object get_current_state() {
                return ((pseudoStateObserver) this).getPosition();
            }
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
        
        static Module() {
            io.add_cube(name, position, sizes, mass);
            time.sleep(1);
            io.add_cube(name1, position1, sizes1, mass);
            io.set_object_position("cube", position: new List<object> {
                0,
                -1,
                1.05
            });
        }
        
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
        
        public static object positionMatrix = new List<object> {
            25,
            20
        };
        
        public static object observer = pseudoStateObserver(poppy, io, name, positionMatrix);
    }
}
