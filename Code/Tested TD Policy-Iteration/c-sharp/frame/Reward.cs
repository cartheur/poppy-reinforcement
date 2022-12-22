namespace Namespace {
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Implementation of the rewards
        public class Reward
            : object {
            
            public object @__metaclass__ = ABCMeta;
            
            // Return the reward for the prev/next action/state combination
            [abstractmethod]
            public virtual object get_rewards(object state, object action, object next_state, object next_action = null) {
            }
        }
    }
}
