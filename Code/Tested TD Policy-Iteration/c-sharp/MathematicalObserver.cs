namespace Namespace {
    
    using CodeFramework;
    
    using System.Diagnostics;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // docstring
        public class MathematicalObserver
            : CodeFramework.StateObserver {
            
            public MathematicalObserver(object state_action_space) {
                Debug.Assert(state_action_space is CodeFramework.GridStateActionSpace2D);
                this.state_action_space = state_action_space;
                this.current_state = (0, 0);
            }
            
            // 
            //         docstring
            //         :return: current_state
            //         
            public virtual object get_current_state() {
                return this.current_state;
            }
        }
    }
}
