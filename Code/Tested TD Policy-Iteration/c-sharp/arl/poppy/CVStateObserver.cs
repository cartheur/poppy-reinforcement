namespace Namespace {
    
    using StateObserver = CodeFramework.StateObserver.StateObserver;
    
    using CVAlgorithm = CVAlgorithm.CVAlgorithm;
    
    using floor = math.floor;
    
    using System.Linq;
    
    using System;
    
    public static class Module {
        
        //  Use computer vision algorithm to observe the agent current state
        public class CVStateObserver
            : StateObserver, CVAlgorithm {
            
            public CVStateObserver(object dimensions = (3, 1)) {
                this.dimensions = dimensions;
            }
            
            //  Return the current state 
            public virtual object get_current_state() {
                var a = ((CVStateObserver) this).getPosition();
                if (a.ToList().Count == 0) {
                    return ValueTuple.Create("<Empty>");
                }
                var _tup_1 = this.dimensions;
                var mm = _tup_1.Item1;
                var nn = _tup_1.Item2;
                if (mm % 2 == 0) {
                    mm += 1;
                }
                if (nn % 2 == 0) {
                    nn += 1;
                }
                var _tup_2 = a;
                var coordinate = _tup_2.Item1;
                var shape = _tup_2.Item2;
                var _tup_3 = coordinate;
                var y = _tup_3.Item1;
                var x = _tup_3.Item2;
                var _tup_4 = shape;
                var n = _tup_4.Item1;
                var m = _tup_4.Item2;
                var unitX = m / mm;
                var unitY = n / nn;
                var xx = floor(x / unitX);
                var yy = floor(y / unitY);
                x = Convert.ToInt32(xx - floor(mm / 2));
                y = Convert.ToInt32(yy - floor(nn / 2));
                return (x, -y);
            }
            
            //  returns if the current state is terminal
            // 
            //         Current implementation: (0,0) is terminal
            // 
            //         :param state: state inquired about
            //         :return: is_terminal: boolean
            //         
            public virtual object is_terminal_state(object state) {
                return state == (0, 0);
            }
        }
        
        public static object dimensions = (5, 3);
        
        public static object observer = CVStateObserver(dimensions);
    }
}
