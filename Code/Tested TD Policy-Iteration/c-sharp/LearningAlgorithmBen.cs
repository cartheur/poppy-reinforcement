namespace Namespace {
    
    using StateActionSpace = CodeFramework.StateActionSpace.StateActionSpace;
    
    using Actor = CodeFramework.Actor.Actor;
    
    using LearningAlgorithm = CodeFramework.LearningAlgorithm.LearningAlgorithm;
    
    using Reward = CodeFramework.Reward.Reward;
    
    using StateObserver = CodeFramework.StateObserver.StateObserver;
    
    using rd = random;
    
    using pyplot = matplotlib.pyplot;
    
    using np = numpy;
    
    using System.Collections.Generic;
    
    using System.Diagnostics;
    
    using System.Linq;
    
    using System;
    
    using rewardSimple = rewardSimple.rewardSimple;
    
    using actorVrep = actorVrep.actorVrep;
    
    using pseudoStateObserver = pseudoStateObserver.pseudoStateObserver;
    
    using MathematicalActor = MathematicalActor.MathematicalActor;
    
    using MathematicalObserver = MathematicalObserver.MathematicalObserver;
    
    using GridStateActionSpace2D = CodeFramework.GridStateActionSpace.GridStateActionSpace2D;
    
    using run_learning = CodeFramework.main.run_learning;
    
    using run_episode = CodeFramework.main.run_episode;
    
    using LearningAlgorithmBen = LearningAlgorithmBen.LearningAlgorithmBen;
    
    using np = numpy;
    
    using time;
    
    using math;
    
    using actorPoppy = ARL_package.PoppyClasses.actorPoppy;
    
    using CVStateObserver = ARL_package.PoppyClasses.CVStateObserver;
    
    using pypot.dynamixel;
    
    public static class Module {
        
        public static object @__author__ = "ben";
        
        public class LearningAlgorithmBen
            : LearningAlgorithm {
            
            public LearningAlgorithmBen(object state_action_space, object Reward, object oldData = new Dictionary<object, object>()) {
                Debug.Assert(state_action_space is StateActionSpace);
                this.figure_count = 1;
                this.state_action_space = state_action_space;
                this.states = state_action_space.get_list_of_states();
                this.actions = state_action_space.get_list_of_actions();
                if (!this.actions.Contains((0, 0))) {
                    this.actions.append((0, 0));
                }
                this.rewardObj = Reward;
                this.epsilon = 0.1;
                if (oldData.has_key("values")) {
                    this.values = oldData["values"];
                } else {
                    this.values = (from x in this.states
                        select 0).ToList();
                }
                this.gamma = 0.7;
                this.learning_rate = 0.3;
                if (oldData.has_key("policy")) {
                    this.policy = oldData["policy"];
                } else {
                    this.policy = (from x in this.states
                        select (0, 0)).ToList();
                    foreach (var stat in this.states) {
                        Console.WriteLine("state", stat);
                        Console.WriteLine("action", this.actions);
                        this.policy[this.states.index(stat)] = this.state_action_space.get_eligible_actions(stat)[0];
                    }
                }
                this.frequencies = (from x in this.states
                    select new Dictionary<object, object>()).ToList();
                foreach (var state in this.states) {
                    var state_freq = this.frequencies[this.states.index(state)];
                    foreach (var action in this.state_action_space.get_eligible_actions(state)) {
                        state_freq.update(new Dictionary<object, object> {
                            {
                                action,
                                new List<object> {
                                    (from x in this.states
                                        select 0).ToList(),
                                    0
                                }}});
                    }
                }
                Console.WriteLine(this.states);
                Console.WriteLine(this.frequencies);
            }
            
            public virtual object get_old_data() {
                var oldData = new Dictionary<object, object>();
                oldData["values"] = this.values;
                oldData["policy"] = this.policy;
                return oldData;
            }
            
            public virtual object compute_expectation(object curr_state, object freq_dict) {
                object probab_currState_action_state;
                var exp_values_per_actions = (from x in this.state_action_space.get_eligible_actions(curr_state)
                    select 0).ToList();
                foreach (var action in this.state_action_space.get_eligible_actions(curr_state)) {
                    var tot_number_counts = freq_dict[action][1];
                    var freq_per_action = freq_dict[action][0];
                    var exp_curr_action = 0;
                    foreach (var state in this.states) {
                        if (tot_number_counts != 0) {
                            probab_currState_action_state = float(freq_per_action[this.states.index(state)]) / float(tot_number_counts);
                        } else {
                            probab_currState_action_state = 1 / float(this.states.Count);
                        }
                        var exp_component = this.rewardObj.get_rewards(curr_state, action, nextState: state, problemType: "capture") + this.gamma * this.values[this.states.index(state)];
                        exp_curr_action += probab_currState_action_state * exp_component;
                    }
                    exp_values_per_actions[this.state_action_space.get_eligible_actions(curr_state).index(action)] = exp_curr_action;
                }
                // print "curr_state: ", curr_state, "freq_dict: ", freq_dict
                // print "expected Values per action: ",  exp_values_per_actions
                return exp_values_per_actions;
            }
            
            public virtual object get_next_action(object current_state) {
                Console.WriteLine("state", current_state);
                if (rd.random() >= 0.1) {
                    // print 'deterministic choice', self.policy[self.states.index(current_state)]
                    Console.WriteLine("action", this.policy[this.states.index(current_state)]);
                    return this.policy[this.states.index(current_state)];
                } else {
                    // print 'epsilon-greedy choice'
                    var a = rd.choice(this.state_action_space.get_eligible_actions(current_state));
                    Console.WriteLine("action", a);
                    return a;
                    // return rd.choice(self.state_action_space.get_eligible_actions(current_state))
                }
            }
            
            // Do TD return
            public virtual object receive_reward(object old_state, object action, object next_state, object reward) {
                var old_state_index = this.states.index(old_state);
                var next_state_index = this.states.index(next_state);
                var td_error = reward + this.gamma * this.values[next_state_index] - this.values[old_state_index];
                this.values[old_state_index] += this.learning_rate * td_error;
                // Update frequencies ~ probabilities
                this.frequencies[old_state_index][action][0][next_state_index] += 1;
                this.frequencies[old_state_index][action][1] += 1;
            }
            
            public virtual object finalise_episode() {
                // print('finalise episode')
                // Update policy
                // if self.global_counter == self.max_number_iterations:
                foreach (var state in this.states) {
                    var state_index = this.states.index(state);
                    var expected_values = this.compute_expectation(state, this.frequencies[state_index]);
                    this.policy[state_index] = this.state_action_space.get_eligible_actions(state)[expected_values.index(max(expected_values))];
                    if (!this.state_action_space.get_eligible_actions(state).Contains(this.policy[state_index])) {
                        Console.WriteLine("ERROR IN FINALISE EPISODE");
                    }
                }
                // print 'policy updated'
                Console.WriteLine(this.policy, this.values);
                // self.plot_results()
            }
            
            public virtual object plot_results() {
                var max_tup_coord = max(this.states);
                var min_tup_coord = min(this.states);
                var min_x = min_tup_coord[0];
                var min_y = min_tup_coord[1];
                var max_x = max_tup_coord[0];
                var max_y = max_tup_coord[1];
                var matr_values = new List<object>();
                foreach (var y in Enumerable.Range(min_y, max_y + 1 - min_y)) {
                    var new_row_values = new List<object>();
                    foreach (var x in Enumerable.Range(min_x, max_x + 1 - min_x)) {
                        new_row_values.append(this.values[this.states.index((x, -y))]);
                    }
                    matr_values.append(new_row_values);
                }
                // http://matplotlib.org/api/pyplot_api.html#matplotlib.pyplot.matshow
                // http://matplotlib.org/examples/pylab_examples/matshow.html
                var VALUES_MAT = np.array(matr_values);
                pyplot.matshow(VALUES_MAT, fignum: this.figure_count, cmap: pyplot.cm.gray);
                pyplot.show();
                this.figure_count += 1;
            }
        }
        
        static Module() {
            @"
    from poppy.creatures import PoppyTorso


    poppy = PoppyTorso(simulator='vrep')

    io = poppy._controllers[0].io
    name = 'cube'
    position = [0, -0.15, 0.85]  # X, Y, Z
    sizes = [0.1, 0.1, 0.1]  # in meters
    mass = 0  # in kg
    io.add_cube(name, position, sizes, mass)
    time.sleep(1)
    name1 = 'cube2'
    position1 = [-0.4, -1, 0.5]
    sizes1 = [3, 1, 1]
    io.add_cube(name1, position1, sizes1, mass)
    io.set_object_position('cube', position=[0, -1, 1.05])
    positionMatrix = (9, 5)
    ";
            run_episode(dummy_actor, dummy_learner, dummy_reward, dummy_observer, dummy_states_actions, max_num_iterations: 100);
            dummy_learner.plot_results();
            run_episode(poppy_actor, dummy_learner, dummy_reward, poppy_observer, dummy_states_actions, max_num_iterations: 100);
            time.sleep(3);
            dummy_learner.plot_results();
        }
        
        public static object positionMatrix = (5, 3);
        
        public static object dummy_states_actions = GridStateActionSpace2D(dimensions: positionMatrix, allow_diag_actions: true);
        
        public static object dummy_observer = MathematicalObserver(dummy_states_actions);
        
        public static object dummy_actor = MathematicalActor(dummy_observer, greedy_epsilon: 0.1);
        
        public static object dummy_reward = rewardSimple();
        
        public static object dummy_learner = LearningAlgorithmBen(dummy_states_actions, dummy_reward);
        
        public static object ports = pypot.dynamixel.get_available_ports();
        
        public static object port = ports[0];
        
        public static object dxl_io = pypot.dynamixel.DxlIO(port);
        
        public static object poppy_observer = CVStateObserver(positionMatrix);
        
        public static object poppy_actor = actorPoppy(dxl_io, positionMatrix);
        
        public static object new_learner = LearningAlgorithmBen(dummy_states_actions, dummy_reward, oldData: dummy_learner.get_old_data());
    }
}
