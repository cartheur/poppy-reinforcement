namespace Namespace {
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // This is where you are told which state it is
        public class StateObserver
            : object {
            
            public object @__metaclass__ = ABCMeta;
            
            // Method that extracts from somewhere which state it is.
            //         :return current_state
            //         
            [abstractmethod]
            public virtual object get_current_state() {
            }
        }
    }
}
