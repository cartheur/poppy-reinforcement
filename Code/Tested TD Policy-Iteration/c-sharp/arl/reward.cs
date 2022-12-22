namespace Namespace {
    
    using Reward = CodeFramework.Reward.Reward;
    
    using np = numpy;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        //  The class gives the setting of reward 
        public class reward
            : Reward {
            
            public reward() {
            }
            
            // This method give back the reward value according to given current
            // 			state, action, next state and if it's a greeting or avoiding case(optional).
            // 			By default is greeting. 
            public virtual object get_rewards(object currentState, object action, object nextState = null, object problemType = "capture") {
                if (problemType == "capture") {
                    if (nextState == (0, 0)) {
                        return 10;
                    }
                    if (nextState == ValueTuple.Create("<Empty>")) {
                        return -100;
                    }
                    return -1;
                }
            }
        }
        
        public static object a = reward();
    }
}
