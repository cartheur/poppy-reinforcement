namespace Namespace {
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Goes from virtual actions to real ones
        public class Actor
            : object {
            
            public object @__metaclass__ = ABCMeta;
            
            // Execute some code to actually do the abstract action 'action' 
            [abstractmethod]
            public virtual object perform_action(object action) {
            }
            
            //  Do initial stuff before episode starts
            //         
            public virtual object initialise_episode() {
            }
        }
    }
}
