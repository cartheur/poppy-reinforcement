namespace Namespace {
    
    using StateActionSpace = CodeFramework.StateActionSpace.StateActionSpace;
    
    using product = itertools.product;
    
    using floor = math.floor;
    
    using System.Collections.Generic;
    
    using System.Linq;
    
    using System;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        // Grid-based state-action space
        // 
        //     Grid based state-action space. Each state and action is a tuple of two
        //     integers. Constructed with the dimensions of the thing.
        public class GridStateActionSpace2D
            : StateActionSpace {
            
            public GridStateActionSpace2D(object dimensions = (3, 1)) {
                this.dimensions = dimensions;
                this.states = this.@__init_states();
                this.actions = this.@__init_actions();
            }
            
            public virtual object @__init_actions() {
                var actionOneDim = new List<object> {
                    -1,
                    0,
                    1
                };
                var actions = (from _tup_1 in product(actionOneDim, repeat: 2).Chop((i,j) => (i, j))
                    let i = _tup_1.Item1
                    let j = _tup_1.Item2
                    select (i, j)).ToList();
                actions.remove((0, 0));
                return actions;
            }
            
            public virtual object @__init_states() {
                var _tup_1 = this.dimensions;
                var m = _tup_1.Item1;
                var n = _tup_1.Item2;
                var indices_x = Convert.ToInt32(floor(m / 2));
                var indices_y = Convert.ToInt32(floor(n / 2));
                var stateOneDimX = Enumerable.Range(-indices_x, indices_x + 1 - -indices_x);
                var stateOneDimY = Enumerable.Range(-indices_y, indices_y + 1 - -indices_y);
                var states = (from _tup_2 in product(stateOneDimX, stateOneDimY).Chop((i,j) => (i, j))
                    let i = _tup_2.Item1
                    let j = _tup_2.Item2
                    select (i, j)).ToList();
                return states;
            }
            
            // Returns a list of all states available
            public virtual object get_list_of_states() {
                return this.states;
            }
            
            // Returns a list of all actions available
            public virtual object get_list_of_actions() {
                return this.actions;
            }
            
            public virtual object get_eligible_actions(object state) {
            }
            
            public virtual object is_terminal_state(object state) {
            }
        }
        
        public static object stateActionSpace = GridStateActionSpace2D((3, 2));
    }
}
