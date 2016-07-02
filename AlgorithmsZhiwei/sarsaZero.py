__author__ = 'Zhiwei Han'

from ARL_package.CodeFramework.LearningAlgorithm import LearningAlgorithm

import numpy as np
import random as rd
import operator

class sarsaZero(LearningAlgorithm):
	""" The Sarsa(0) algorithm dessign. """
	def __init__(self, problem, epsilonGreedy, numEpisoid, alpha, gamma):
		self.epsilonGreedy = epsilonGreedy
		self.numEpisoid = numEpisoid
		self.alpha = alpha
		self.gamma = gamma

		self.problem = problem

		self.stateSpace = self.problem.get_list_of_states()  		# Initialize the state list
		self.actionSpace = self.problem.get_list_of_actions()		# Initialize the action list
		self.qFunc = self.initializeQFunc()						# Initialize the Q function

	def initializeQFunc(self):
		""" Initialize the Q function 
			used in constructor """
		qFunc = {}
		for i in self.stateSpace:
			qFunc[i] = {j:0 for j in self.actionSpace[i]}
		return qFunc

	def get_next_action(self, currentState):
		""" Pick the action from action space with given state by 
			using epsilon greedy method """
		if rd.random() < self.epsilonGreedy:
			actions = list(self.actionSpace[currentState])
			action = actions[rd.randint(0, len(actions) - 1)]
			return action
		else:
			actions = self.qFunc[currentState]
			action = max(actions.iteritems(), key=operator.itemgetter(1))[0]
			return action

	def updateQFunc(self, currentState, action, reward, nextState, nextAction):
		""" Update the Q function with given current state and 
				next state by weighted TD error """
		if len(list(nextState)) != 0:
			tdError = reward + self.gamma * self.qFunc[nextState][nextAction] - self.qFunc[currentState][action]
			self.qFunc[currentState][action] = round(self.qFunc[currentState][action] + self.alpha * tdError, 3)
		else:
			tdError = reward + self.gamma * -100 - self.qFunc[currentState][action]
			self.qFunc[currentState][action] = round(self.qFunc[currentState][action] + self.alpha * tdError, 3)

	def trainEpisoid(self):
		""" Train the model with only one episoid and 
			use the predefined function update the Q
			function in every step """
		ifTerminal = lambda x: list(x)[0] == list(x)[1] == 0

		visitedStates = []
		currentState = self.problem.get_initial_state()
		visitedStates.append(currentState)
		step = 0
		totalReward = 0

		action = self.get_next_action(currentState)
		while len(list(currentState)) != 0 and not ifTerminal(currentState):  	# Check if the algorithm has reached the terminal
			self.problem.perform_action(currentState, action)						# or the object is already out of sight 
			nextState = self.problem.get_current_state()
			visitedStates.append(nextState)
			reward = self.problem.get_reward(currentState, action, nextState)
			totalReward += reward

			if len(list(nextState)) == 0:										# If next state is out of sight then no further action can be taken
				nextAction = ()
			else:
				nextAction = self.get_next_action(nextState)

			self.updateQFunc(currentState, action, reward, nextState, nextAction)	# Update Q function with the new knowledge of reward

			currentState = nextState
			
			action = nextAction

			step += 1
		print visitedStates
		print 'The episoid has been done in ', step, 'step'
		return step, totalReward, visitedStates

	def trainModel(self):
		for i in xrange(self.numEpisoid):
			print 'Trace', i + 1, ':'
			self.trainEpisoid()
			print 'Q Function'
			for j in self.qFunc:
				print j, ':', self.qFunc[j]
			self.epsilonGreedy *= 0.99

	def derivePolicy(self):
		policy = {}
		stateSpace = self.stateSpace
		stateSpace.remove((0, 0))
		for i in stateSpace:
			optimalAction = max(self.qFunc[i].iteritems(), key=operator.itemgetter(1))[0]
			policy[i] = optimalAction
		return policy

	def getPolicy(self):
		return  self.derivePolicy()

	def receive_reward(self):
		pass