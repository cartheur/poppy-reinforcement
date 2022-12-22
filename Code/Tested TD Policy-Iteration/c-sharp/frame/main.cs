namespace Namespace {
    
    using System.Diagnostics;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Run the main loop!
        public static object run_learning(object actor, object learning_algorithm, object reward, object state_observer) {
            // Check that the variables given have the right superclasses
            Debug.Assert(actor is Actor);
            Debug.Assert(learning_algorithm is LearningAlgorithm);
            Debug.Assert(reward is Reward);
            Debug.Assert(state_observer is StateObserver);
            var current_state = state_observer.get_current_state();
            while (current_state) {
                var next_action = learning_algorithm.get_next_action(current_state);
                actor.perform_action(next_action);
                var next_state = state_observer.get_current_state();
                var reward_given = reward.get_rewards(current_state, next_action, next_state);
                learning_algorithm.receive_reward(current_state, next_action, next_state, reward_given);
                current_state = next_state;
            }
        }
        
        public static object run_episode(
            object actor,
            object learning_algorithm,
            object reward,
            object state_observer,
            object state_action_space,
            object max_num_iterations = 1000000) {
            // assert (isinstance(actor, Actor))
            // assert (isinstance(learning_algorithm, LearningAlgorithm))
            // assert (isinstance(reward, Reward))
            // assert (isinstance(state_observer, StateObserver))
            // assert isinstance(state_action_space, StateActionSpace)
            actor.initialise_episode(state_action_space);
            var current_state = state_observer.get_current_state();
            var current_iter = 0;
            while (current_iter < max_num_iterations) {
                var next_action = learning_algorithm.get_next_action(current_state);
                actor.perform_action(next_action);
                var next_state = state_observer.get_current_state();
                var reward_given = reward.get_rewards(current_state, next_action, next_state);
                learning_algorithm.receive_reward(current_state, next_action, next_state, reward_given);
                current_state = next_state;
                current_iter += 1;
                if (state_action_space.is_terminal_state(current_state)) {
                    reward_given = reward.get_rewards(current_state, (0, 0), next_state);
                    learning_algorithm.receive_reward(current_state, (0, 0), next_state, reward_given);
                    break;
                }
            }
            learning_algorithm.finalise_episode();
            Console.WriteLine("run_episode: Episode ended after " + current_iter.ToString() + " iterations.");
        }
        
        public static object dummy_states_actions = DummyStateActionSpace();
        
        static Module() {
            dummy_states_actions.states[0] = 2;
            run_learning(dummy_actor, dummy_learner, dummy_reward, dummy_observer);
        }
        
        public static object dummy_actor = DummyActor();
        
        public static object dummy_observer = DummyObserver(dummy_states_actions, dummy_actor);
        
        public static object dummy_learner = DummyLearner(dummy_states_actions);
        
        public static object dummy_reward = DummyReward(dummy_states_actions);
    }
}
