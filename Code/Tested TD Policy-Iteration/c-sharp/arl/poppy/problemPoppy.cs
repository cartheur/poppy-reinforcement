namespace Namespace {
    
    using TrainningWorld = CodeFramework.TrainningWorld.TrainningWorld;
    
    using GridStateActionSpace2D = GridStateActionSpace2D.GridStateActionSpace2D;
    
    using CVStateObserver = CVStateObserver.CVStateObserver;
    
    using actorPoppy = actorPoppy.actorPoppy;
    
    using reward = reward.reward;
    
    using time;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        //  A trainning world of agent and enviroment.
        // 		The main and only object interacts with learning algorithm
        // 		during learning. 
        public class problemPoppy
            : TrainningWorld {
            
            public problemPoppy(object CVStateObserver, object actorPoppy, object reward, object GridStateActionSpace2D) {
                this.CVStateObserver = CVStateObserver;
                this.actorPoppy = actorPoppy;
                this.reward = reward;
                this.GridStateActionSpace2D = GridStateActionSpace2D;
                this.stateSpace = this.GridStateActionSpace2D.get_list_of_states();
                this.actionSpace = this.GridStateActionSpace2D.get_list_of_actions();
                this.reward = reward;
            }
            
            //  Return the current state 
            public virtual object get_current_state() {
                return this.CVStateObserver.get_current_state();
            }
            
            public virtual object get_initial_state() {
                this.actorPoppy.come_to_zero();
                time.sleep(3);
                var currentState = this.CVStateObserver.get_current_state();
                while (currentState == (0, 0) || currentState == ValueTuple.Create("<Empty>")) {
                    currentState = this.get_current_state;
                    time.sleep(1);
                }
            }
            
            public virtual object get_list_of_states() {
                return this.stateSpace;
            }
            
            public virtual object get_list_of_actions() {
                return this.actionSpace;
            }
            
            public virtual object perform_action(object currentState, object action) {
                this.actorPoppy.perform_action(action);
            }
            
            public virtual object get_reward(object currentState, object action, object nextState) {
                return this.reward.get_rewards(currentState, action, nextState);
                // def getSuccessor(self, currentState, action):
                // 	diffX, diffY = action
                // 	x, y = currentState
                // 	newX, newY = x + diffX, y + diffY
                // 	return (newX, newY)
                // 	x, y = currentState
                // 	diffX, diffY = action
                // 	nextState = (x + diffX, y + diffY)
                // 	return reward.get_rewards(currentState, action, nextState)
            }
        }
    }
}
