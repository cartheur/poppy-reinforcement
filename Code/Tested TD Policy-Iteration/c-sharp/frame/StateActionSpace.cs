namespace Namespace {
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using System.Collections.Generic;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Manager for states and actions available
        public class StateActionSpace
            : object {
            
            public object @__metaclass__ = ABCMeta;
            
            // Returns a list of all states available
            [abstractmethod]
            public virtual object get_list_of_states() {
            }
            
            // Returns a list of all actions available
            [abstractmethod]
            public virtual object get_list_of_actions() {
            }
            
            // Returns eligible actions for state state
            [abstractmethod]
            public virtual object get_eligible_actions(object state) {
            }
            
            // 
            //         returns if the current state is terminal
            //         :param state: state inquired about
            //         :return: is_terminal: boolean
            //         
            public virtual object is_terminal_state(object state) {
                return @bool(state);
            }
            
            // 
            //         Gets the representation of the terminal state
            // 
            //         :return: terminal state
            //         
            public virtual object get_terminal_state() {
                return new List<object>();
            }
        }
    }
}
