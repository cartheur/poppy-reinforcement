namespace Namespace {
    
    using np = numpy;
    
    using System.Linq;
    
    using System.Collections.Generic;
    
    using System;
    
    public static class Module {
        
        public static object @__author__ = "erik";
        
        // Grid-based state-action space
        // 
        //     Grid based state-action space. Each state and action is a tuple of two
        //     integers. Constructed with the dimensions of the thing.
        public class GridStateActionSpace2D
            : StateActionSpace {
            
            public GridStateActionSpace2D(object dimensions = (3, 1), object allow_diag_actions = true) {
                var half_dimensions = (np.array(dimensions) / 2).astype(@int);
                this.min_indices = -half_dimensions;
                this.max_indices = this.min_indices + dimensions - (1, 1);
                @"All States";
                this.states = (from _tup_1 in np.ndindex(dimensions).Chop((i,j) => (i, j))
                    let i = _tup_1.Item1
                    let j = _tup_1.Item2
                    select ((i - half_dimensions[0]), (j - half_dimensions[1]))).ToList();
                Func<object, object, object, object> @__generate_possible_actions = (dims,state_index,allow_diag) => {
                    if (state_index == (0, 0)) {
                        return new List<object> {
                            (0, 0)
                        };
                    }
                    var min_indices = -(np.array(dims) / 2).astype(@int);
                    var max_indices = min_indices + dims - (1, 1);
                    // disallow going out of edges
                    var min_action = -np.less(min_indices, state_index).astype(@int);
                    var max_action = np.less(state_index, max_indices).astype(@int);
                    // have same actions everywhere
                    // min_action = -(np.less(min_indices, (0, 0))).astype(int)
                    // max_action = np.less((0, 0), max_indices).astype(int)
                    var actions = new List<object>();
                    foreach (var i in Enumerable.Range(min_action[0], max_action[0] + 1 - min_action[0])) {
                        foreach (var j in Enumerable.Range(min_action[1], max_action[1] + 1 - min_action[1])) {
                            if ((i * j == 0 || allow_diag) && (i, j) != (0, 0)) {
                                actions.append((i, j));
                            }
                        }
                    }
                    return actions;
                };
                this.eligible_actions = new Dictionary<object, object>();
                foreach (var state in this.states) {
                    this.eligible_actions[state] = @__generate_possible_actions(dimensions, state_index: state, allow_diag: allow_diag_actions);
                }
                this.actions = this.eligible_actions[(1, 0)];
            }
            
            // Returns a list of all states available
            public virtual object get_list_of_states() {
                return this.states;
            }
            
            // Returns a list of all actions available
            public virtual object get_list_of_actions() {
                return this.actions;
            }
            
            // Returns eligible actions for state state
            public virtual object get_eligible_actions(object state) {
                return this.eligible_actions[state];
            }
            
            // returns if the current state is terminal
            // 
            //         Current implementation: (0,0) is terminal
            // 
            //         :param state: state inquired about
            //         :return: is_terminal: boolean
            //         
            public virtual object is_terminal_state(object state) {
                return state == (0, 0);
            }
        }
    }
}
