namespace Namespace {
    
    using StateActionSpace = StateActionSpace.StateActionSpace;
    
    using Actor = Actor.Actor;
    
    using LearningAlgorithm = LearningAlgorithm.LearningAlgorithm;
    
    using Reward = Reward.Reward;
    
    using StateObserver = StateObserver.StateObserver;
    
    using System.Collections.Generic;
    
    using System.Diagnostics;
    
    using System.Linq;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        public class DummyStateActionSpace
            : StateActionSpace {
            
            public DummyStateActionSpace() {
                this.states = new List<object> {
                    0,
                    1,
                    2,
                    3
                };
                this.actions = new List<object> {
                    7,
                    4
                };
            }
            
            public virtual object get_list_of_states() {
                return this.states;
            }
            
            public virtual object get_list_of_actions() {
                return this.actions;
            }
            
            public virtual object get_eligible_actions(object state) {
                if (state == this.states[0]) {
                    return new List<object> {
                        this.actions[1]
                    };
                } else {
                    return new List<object> {
                        this.actions[0]
                    };
                }
            }
        }
        
        public class DummyActor
            : Actor {
            
            public DummyActor() {
                this.a_variable = 1;
            }
            
            public virtual object perform_action(object action) {
                this.a_variable += 1;
                Console.WriteLine("DummyActor performs action " + action.ToString());
            }
        }
        
        public class DummyObserver
            : StateObserver {
            
            public DummyObserver(object state_action_space, object dummy_actor) {
                Debug.Assert(state_action_space is StateActionSpace);
                Debug.Assert(dummy_actor is DummyActor);
                this.states = state_action_space.get_list_of_states();
                this.whichState = 0;
                this.dummy_actor = dummy_actor;
            }
            
            public virtual object get_current_state() {
                this.whichState = this.dummy_actor.a_variable % 2;
                return this.states[this.whichState];
            }
        }
        
        public class DummyReward
            : Reward {
            
            public DummyReward(object state_action_space) {
                this.actions = state_action_space.get_list_of_actions();
            }
            
            public virtual object get_rewards(object state, object action, object next_state, object next_action = null) {
                if (state == (0, 0)) {
                    // reward being in the terminal state
                    return 1000;
                } else {
                    return 0;
                }
            }
        }
        
        public class DummyLearner
            : LearningAlgorithm {
            
            public DummyLearner(object state_action_space) {
                Debug.Assert(state_action_space is StateActionSpace);
                this.state_action_space = state_action_space;
                this.states = state_action_space.get_list_of_states();
                this.actions = state_action_space.get_list_of_actions();
                this.gamma = 0.5;
                this.learning_rate = 0.1;
                //random initialisation
                this.values = new Dictionary<object, object>();
                this.policy = new Dictionary<object, object>();
                foreach (var _tup_1 in this.states.Select((_p_1,_p_2) => Tuple.Create(_p_2, _p_1))) {
                    var i = _tup_1.Item1;
                    var state = _tup_1.Item2;
                    this.values[state] = 0;
                    var eligible_actions = this.state_action_space.get_eligible_actions(state);
                    this.policy[state] = eligible_actions[i % eligible_actions.Count];
                }
                Console.WriteLine(this.values);
            }
            
            public virtual object get_next_action(object current_state) {
                return this.policy[current_state];
            }
            
            // Do TD return
            public virtual object receive_reward(object old_state, object action, object next_state, object reward) {
                var td_error = reward + this.gamma * this.values[next_state] - this.values[old_state];
                this.values[old_state] += this.learning_rate * td_error;
            }
            
            // 
            //         Update policy greedily, in the dummy example assuming equal transition probabilities
            //         
            public virtual object finalise_episode() {
                var next_state_deterministic = (current_state,next_action) => (current_state[0] + next_action[0], current_state[1] + next_action[1]);
                try {
                    foreach (var state in this.policy.keys()) {
                        var current_next_value = this.values[next_state_deterministic(state, this.policy[state])];
                        // find best action
                        foreach (var action in this.state_action_space.get_eligible_actions(state)) {
                            var value_of_next = this.values[next_state_deterministic(state, action)];
                            if (value_of_next >= current_next_value) {
                                this.policy[state] = action;
                                current_next_value = value_of_next;
                            }
                        }
                    }
                } catch (TypeError) {
                    //<parser-error>
                }
            }
        }
    }
}
