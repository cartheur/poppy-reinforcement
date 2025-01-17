import numpy as np
from numpy.linalg import norm
import matplotlib.pyplot as plt
from copy import deepcopy

class PlotAgentValueFunc(object):
	def __init__(self, dimension):
		"""
		Diagramm:
			Horizontal: Episoid Number
			Vertical:
				1. Step Number
				2. Total Reward
				3. If reach center
				4. Q function difference every 100 episoid
		Graph:
			Policy after 100 Episoid
		"""
		self.dimension = dimension

	def get_valueFunc_diff(self, valueFuncHistory):
		valueFuncDiff = []
		valueFuncPre = []
		valueFuncCurrent = []
		for i in valueFuncHistory:
			valueFuncPre = valueFuncCurrent
			valueFuncCurrent = i
			if len(list(valueFuncPre)) == 0:
				continue
			temp = 0
			temp += (np.array(valueFuncPre.values(), dtype=float) - np.array(valueFuncCurrent.values(), dtype=float)) ** 2
			valueFuncDiff.append(np.sqrt(sum(temp)))
			valueFuncPre = valueFuncCurrent
		return valueFuncDiff

	def plot_policy_graph(self, policyHistory):
		for singlePolicy in reversed(policyHistory):
			soaList = []
			for state in singlePolicy.keys():
				if state == (0, 0):
					continue
				action = singlePolicy[state]
				x, y = state
				m, n = action
				soaList.append([x, y, m, n])
			X,Y,U,V = zip(*soaList)
			plt.figure()
			ax = plt.gca()
			ax.quiver(X,Y,U,V,angles='xy',scale_units='xy',scale=1)
			ax.set_xlim([-list(self.dimension)[0] // 2, 
				list(self.dimension)[0] // 2 + 1])
			ax.set_ylim([-list(self.dimension)[1] // 2, 
				list(self.dimension)[1] // 2 + 1])
			break
		plt.draw()
		plt.show()

	def plot_diag(self, diagInfo, valueFuncDiff):
		stepNumTrue = []
		totalReward = []
		ifReachCenter = []
		for i in diagInfo:
			stepNumTrue.append(list(i)[0])
			totalReward.append(list(i)[1])
			ifReachCenter.append(list(i)[2])
		stepNumFalse = deepcopy(stepNumTrue)

		for i in xrange(len(ifReachCenter)):
			if ifReachCenter[i]:
				stepNumFalse[i] = 0
			else:
				stepNumTrue[i] = 0


		length = np.arange(1, len(diagInfo) + 1)
		plt.subplot(3, 1, 1)
		plt.plot(length, stepNumFalse, 'r')
		plt.plot(length, stepNumTrue, 'b')
		plt.title('How many steps does learning algorithm need to reach Terminal or failed')
		plt.ylabel('Step Number')

		# plt.subplot(4, 1, 2)
		# plt.plot(length, ifReachCenter, 'r.-')
		# plt.title('If the agent reach the goal state')
		# plt.ylabel('1 for Reaching')
		# plt.xlabel('Episoid')

		# # ifReachCenter = np.array(ifReachCenter, dtype=bool) * 1
		# # ifReachCenter = np.array(ifReachCenter, dtype=int)
		plt.subplot(3, 1, 2)
		plt.plot(length, totalReward, 'k')
		plt.title('How much is the total reward in one episoid')
		plt.ylabel('Reward')

		length = np.arange(1, len(valueFuncDiff) + 1)
		plt.subplot(3, 1, 3)
		plt.plot(length, valueFuncDiff, 'g-')
		plt.title('How big is the difference of Q function in every 10 episoids')
		plt.ylabel('Difference')

		plt.show()


	def plot(self, diagInfo, valueFuncHistory, policyHistory):
		valueFuncDiff = self.get_valueFunc_diff(valueFuncHistory)
		self.plot_diag(diagInfo, valueFuncDiff)
		self.plot_policy_graph(policyHistory)

if __name__ == '__main__':
	import numpy as np
	import matplotlib.pyplot as plt
	soa =np.array( [ [0,0,3,2], [0,0,1,1],[0,0,9,9]]) 
	X,Y,U,V = zip(*soa)
	plt.figure()
	ax = plt.gca()
	ax.quiver(X,Y,U,V,angles='xy',scale_units='xy',scale=1)
	ax.set_xlim([-1,10])
	ax.set_ylim([-1,10])
	plt.draw()
	plt.show()