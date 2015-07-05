
var GVF = function (){

	var gvf = {
		setDimensions   :  Module.cwrap('setDimensions', null, ['number']),
		setSegmentation :  Module.cwrap('setSegmentation', null, ['number']),
		setMultiPoint :  Module.cwrap('setMultiPoint', null, ['number']),
		setRotationFeatures :  Module.cwrap('setRotationFeatures', null, ['number']),

		initGVF :  Module.cwrap('initGVF', null, []),

		addObservation2D :  Module.cwrap('addObservation2D', null, ['number', 'number']),
		getNumObservations :  Module.cwrap('getNumObservations', 'number', ['number']),
		getObservation_internal :  Module.cwrap('getObservation', null, ['number', 'number']),
		getObservationVal_internal :  Module.cwrap('getObservationVal', 'number', ['number']),
		getObservationCurrentGestures_internal :  Module.cwrap('getObservationCurrentGestures', null, ['number']),
		getNuObservationsCurrentGestures :  Module.cwrap('getNumObservationsCurrentGestures', 'number', []),
		

		getObservation : function (gestureNum, observationNum){
			this.getObservation_internal(gestureNum, observationNum);
			return {
				x : this.getObservationVal_internal(0),
				y : this.getObservationVal_internal(1)
			};
		},

		getObservationZeroOrigin : function (gestureNum, observationNum){
			this.getObservation_internal(gestureNum, observationNum);
			obs =  {
				x : this.getObservationVal_internal(0),
				y : this.getObservationVal_internal(1)
			};
			this.getObservation_internal(gestureNum, 0);
			obs.x -= this.getObservationVal_internal(0);
			obs.y -= this.getObservationVal_internal(1);
			return obs;
		},

		getObservationCurrentGesture : function (observationNum){
			this.getObservationCurrentGestures_internal(observationNum);
			return {
				x : this.getObservationVal_internal(0),
				y : this.getObservationVal_internal(1)
			};
		},

		getObservationZeroOriginCurrentGesture : function (observationNum){
			this.getObservationCurrentGestures_internal(gestureNum, observationNum);
			obs =  {
				x : this.getObservationVal_internal(0),
				y : this.getObservationVal_internal(1)
			};
			this.getObservationCurrentGestures_internal(gestureNum, 0);
			obs.x -= this.getObservationVal_internal(0);
			obs.y -= this.getObservationVal_internal(1);
			return obs;
		},

		infer :  Module.cwrap('infer', null, []),

		getNumberOfGestureTemplates :  Module.cwrap('getNumberOfGestureTemplates', 'number', []),
	 	
		getMostProbable :  Module.cwrap('getMostProbable', 'number', []),
		getProbability  :  Module.cwrap('getProbability', 'number', ['number']),
		getPhase        :  Module.cwrap('getPhase', 'number', ['number']),
		getSpeed        :  Module.cwrap('getSpeed', 'number', ['number']),
		getScale        :  Module.cwrap('getScale', 'number', ['number']),
		getRotation     :  Module.cwrap('getRotation', 'number', ['number']),


		startLearning       :  Module.cwrap('startLearning', null, []),
		endLearning         :  Module.cwrap('endLearning', null, []),
		isLearning          :  Module.cwrap('isLearning', 'number', []),
		setGestureToCurrent :  Module.cwrap('setGestureToCurrent', null, ['number']),
		startFollowing      :  Module.cwrap('startFollowing', null, []),
		endFollowing        :  Module.cwrap('endFollowing', null, []),
		isFollowing         :  Module.cwrap('isFollowing', 'number', []),

		setTolerance       :  Module.cwrap('setTolerance', null, ['number']),
		getTolerance       :  Module.cwrap('getTolerance', 'number', []),
		setPhaseVariance   :  Module.cwrap('setPhaseVariance', null, ['number']),
		getPhaseVariance   :  Module.cwrap('getPhaseVariance', 'number', []),
		setScaleVariance   :  Module.cwrap('setScaleVariance', null, ['number']),
		getScaleVariance   :  Module.cwrap('getScaleVariance', 'number', []),
		setSpeedVariance   :  Module.cwrap('setSpeedVariance', null, ['number']),
		getSpeedVariance   :  Module.cwrap('getSpeedVariance', 'number', []),
		setRotationVariance   :  Module.cwrap('setRotationVariance', null, ['number']),
		getRotationVariance   :  Module.cwrap('getRotationVariance', 'number', []),

		saveTemplates   :  Module.cwrap('saveTemplates', null, ['string']),
		loadTemplates   :  Module.cwrap('loadTemplates', null, ['string']),

		clear   :  Module.cwrap('clear', null, []),

	}; 

	return gvf;
}