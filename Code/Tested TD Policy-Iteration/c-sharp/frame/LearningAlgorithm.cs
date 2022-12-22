namespace Namespace {
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Implementation class of the Learning Algorithm
        public class LearningAlgorithm
            : object {
            
            public object @__metaclass__ = ABCMeta;
            
            // Find out which is the next action given the state 'current_state'
            //                 :return next_action : next action according to policy
            //                         
            [abstractmethod]
            public virtual object get_next_action(object current_state) {
            }
            
            // Perform things according to which reward was given
            [abstractmethod]
            public virtual object receive_reward(object old_state, object action, object next_state, object reward) {
            }
            
            // Do things that need to be done when an episode was finished
            public virtual object finalise_episode() {
            }
        }
    }
}
