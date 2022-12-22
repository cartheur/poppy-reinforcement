namespace Namespace {
    
    using IfTerminal = CodeFramework.IfTerminal.IfTerminal;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        // Goes from virtual actions to real ones
        public class isTerminal
            : IfTerminal {
            
            // Execute some code to actually do the abstract action 'action' 
            public virtual object is_terminal(object state) {
                return state == (0, 0);
            }
        }
    }
}
