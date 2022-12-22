namespace Namespace {
    
    using CodeFramework;
    
    using MathematicalObserver = MathematicalObserver.MathematicalObserver;
    
    using rd = random;
    
    using System.Diagnostics;
    
    using random;
    
    using System;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Deterministic Actor that prints every action it takes
        public class MathematicalActor
            : CodeFramework.Actor {
            
            public MathematicalActor(object mathematicalObserver, object greedy_epsilon = 0) {
                Debug.Assert(mathematicalObserver is MathematicalObserver);
                this.mathematicalObserver = mathematicalObserver;
                this.epsilon = greedy_epsilon;
                if (greedy_epsilon > 0) {
                    // only import 'random' library if we want epsilon-greedy
                    random.seed();
                    this.probabilistic_event = probabilistic_event;
                    this.random_choice = random_choice;
                }
                Func<object, object> probabilistic_event = probability => {
                    return random.random() < probability;
                };
                Func<object, object> random_choice = num_choices => {
                    return Convert.ToInt32(random.random() * num_choices);
                };
            }
            
            // 
            //         Change state in MathematicalObserver and print
            // 
            //         :param action: tuple
            //         :return:
            //         
            public virtual object perform_action(object action) {
                object next_state;
                var current_state = this.mathematicalObserver.get_current_state();
                var eligible_actions = this.mathematicalObserver.state_action_space.get_eligible_actions(current_state);
                if (eligible_actions.Contains(action)) {
                    if (this.epsilon == 0 || !this.probabilistic_event(this.epsilon)) {
                        next_state = (current_state[0] + action[0], current_state[1] + action[1]);
                        // print "MathematicalActor: Executing policy-action " + str(action) + \
                        //      " from " + str(current_state) + " to " + str(next_state)
                        this.mathematicalObserver.current_state = next_state;
                    } else {
                        var num_choices = eligible_actions.Count;
                        var choice = this.random_choice(num_choices);
                        var random_action = eligible_actions[choice];
                        next_state = (current_state[0] + random_action[0], current_state[1] + random_action[1]);
                        // print "MathematicalActor: Executing epsilon-random action" + str(random_action) + \
                        //      " from " + str(current_state) + " to " + str(next_state)
                        this.mathematicalObserver.current_state = next_state;
                    }
                } else {
                    Console.WriteLine("MathematicalActor: From state " + current_state.ToString() + ", action " + action.ToString() + " is not eligible");
                    throw ValueError;
                }
            }
            
            // 
            //         Don't need to do anything, really.
            //         :return:
            //         
            public virtual object initialise_episode(object state_action_space) {
                this.mathematicalObserver.current_state = rd.choice(this.mathematicalObserver.state_action_space.states);
            }
        }
    }
}
