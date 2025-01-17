__author__ = 'ben'

from ARL_package import CodeFramework, VrepClasses, Learning, Rewards, MathematicalClasses, PoppyClasses
import matplotlib.pyplot as plt
import numpy as np
import pypot.dynamixel
# Added Poppy-Classes as well to have a running version in place

positionMatrix = (9, 7)
moving_average = list([])
mov_avg_number = 10
num_episodes = 50
epsilon = 0.1
gamma = 0.7
learning_rate = 0.1

number_of_test_runs = 1
rewards_time_series = list()

for j in xrange(number_of_test_runs):
    rewards = list([])
    # Step 1 - Mathematical Pre-Training
    states_actions = CodeFramework.GridStateActionSpace2D(dimensions=positionMatrix, allow_diag_actions=True)
    observer = MathematicalClasses.MathematicalObserver(states_actions)
    actor = MathematicalClasses.MathematicalActor(observer,greedy_epsilon=0) # as the learning algrotithm already has its own epsiolon-greedy behaviour, this was not needed.
    # actor = VrepClasses.MathematicalActorExtended(observer, greedy_epsilon=0.0)
    reward = Rewards.RewardZhiwei(states_actions)
    learner = Learning.TDPolicyIteration(states_actions, reward, epsilon, gamma, learning_rate)



    for i in xrange(num_episodes):
        rewards.append( CodeFramework.main.run_episode(actor, learner, reward, observer, states_actions, max_num_iterations=100) )
        if i >= mov_avg_number:
            moving_average.append( np.mean(np.array(rewards)[i-mov_avg_number:i+1]) )

    rewards_time_series.append(np.array(rewards))

    print "Finished run {} of {}".format(j, number_of_test_runs)


np.savetxt('data.csv', np.array(rewards_time_series).T, delimiter=',')

# plt.plot( np.array(range(num_episodes-mov_avg_number)), np.array(moving_average) )
# plt.show()

print 'Current_state: ', observer.get_current_state()
print "Values: ", str(learner.values)

learner.plot_results()

# """
# Step 2 - Vrep-Simulation
poppy_observer = VrepClasses.ObserverVrep(states_actions, positionMatrix)
poppy_actor = VrepClasses.ActorVrep(poppy_observer)

new_learner = Learning.TDPolicyIteration(states_actions, reward, epsilon, gamma, learning_rate, oldData=learner.get_old_data())

for i in range(50):
    CodeFramework.main.run_episode(poppy_actor, new_learner, reward, poppy_observer, states_actions, max_num_iterations=100)

learner.plot_results()

print 'Current_state: ', observer.get_current_state()
print "Values: ", str(learner.values)
#"""
