namespace Namespace {
    
    using Reward = CodeFramework.Reward.Reward;
    
    using np = numpy;
    
    using System.Collections.Generic;
    
    using System.Linq;
    
    using System;
    
    public static class Module {
        
        public static object @__author__ = "ben";
        
        //  The class gives the setting of reward 
        public class rewardSimple
            : Reward {
            
            public rewardSimple() {
            }
            
            public virtual object get_rewards(object currentState, object action, object nextState, object problemType = "capture") {
                if (nextState == new List<object>()) {
                    return -10;
                } else if (nextState == (0, 0)) {
                    return 1;
                } else {
                    return 0;
                }
            }
            
            // This method give back the reward value according to given current
            //             state, action, next state and if it's a greeting or avoiding case(optional).
            //             By default is greeting. 
            public virtual object get_rewards_(object currentState, object action, object nextState = null, object problemType = "capture") {
                object euclideanDis2;
                object euclideanDis1;
                // changed to correct function name
                if (currentState.ToList().Count == 0 && nextState.ToList().Count == 0) {
                    return 0;
                }
                if (problemType == "capture") {
                    if (currentState.ToList().Count == 0) {
                        if (nextState.ToList().Count != 0) {
                            return 0;
                        }
                    }
                    if (currentState.ToList().Count != 0) {
                        if (nextState.ToList().Count == 0) {
                            return -100;
                        }
                    }
                    euclideanDis1 = Math.Pow(currentState.ToList()[0], 2) + Math.Pow(currentState.ToList()[1], 2);
                    euclideanDis2 = Math.Pow(nextState.ToList()[0], 2) + Math.Pow(nextState.ToList()[1], 2);
                    var absReward = np.sqrt(euclideanDis1) - np.sqrt(euclideanDis2);
                    if (absReward > 0) {
                        return round(absReward, 3);
                    } else {
                        return round(absReward, 3) - 1;
                    }
                } else {
                    if (currentState.ToList().Count == 0) {
                        if (nextState.ToList().Count != 0) {
                            return -100;
                        }
                    }
                    if (currentState.ToList().Count != 0) {
                        if (nextState.ToList().Count == 0) {
                            return 100;
                        }
                    }
                    euclideanDis1 = Math.Pow(currentState.ToList()[0], 2) + Math.Pow(currentState.ToList()[1], 2);
                    euclideanDis2 = Math.Pow(nextState.ToList()[0], 2) + Math.Pow(nextState.ToList()[1], 2);
                    if (absReward < 0) {
                        return -round(absReward, 3);
                    } else {
                        return -round(absReward, 3) - 1;
                    }
                }
            }
        }
        
        public static object a = rewardSimple();
    }
}
