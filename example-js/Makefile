
EXPORTED_FUNCTIONS="['_setDimensions', '_setSegmentation', '_setMultiPoint', '_setRotationFeatures', '_initGVF', '_addObservation2D', '_getNumObservations', '_getObservation', '_getObservationVal', '_getNumObservationsCurrentGestures', '_getObservationCurrentGestures', '_infer', '_getNumberOfGestureTemplates', '_getMostProbable', '_getProbability', '_getPhase', '_getSpeed', '_getScale', '_getRotation', '_startLearning', '_endLearning', '_setGestureToCurrent', '_isLearning', '_startFollowing', '_endFollowing', '_isFollowing', '_setTolerance', '_getTolerance', '_setPhaseVariance', '_getPhaseVariance', '_setScaleVariance', '_getScaleVariance', '_setSpeedVariance', '_getSpeedVariance', '_setRotationVariance', '_getRotationVariance', '_saveTemplates', '_loadTemplates', '_clear']"

GVF.js: GVFjs.cpp GVFjs.h ../src/ofxGVF.h ../src/ofxGVFTypes.h ../src/ofxGVFGesture.h ../src/ofxGVF.cpp
	emcc -DEMSCRIPTEN -g2 -s ASSERTIONS=2 -s DEMANGLE_SUPPORT=1 -s SAFE_HEAP=1  -I../src -std=c++11 ../src/ofxGVF.cpp GVFjs.cpp  --post-js GVF_wrapper.js -o js/GVF.js -s EXPORTED_FUNCTIONS=$(EXPORTED_FUNCTIONS)
	#emcc -DEMSCRIPTEN -g2 -s ASSERTIONS=2 -s DEMANGLE_SUPPORT=1 -s SAFE_HEAP=1  -I../src -std=c++11 GVFjs.cpp ../src/ofxGVF.cpp  --post-js GVF_wrapper.js -o js/GVF.js -s EXPORTED_FUNCTIONS=$(EXPORTED_FUNCTIONS)


GVF.min.js: GVFjs.cpp GVFjs.h ../src/ofxGVF.h ../src/ofxGVFTypes.h ../src/ofxGVFGesture.h ../src/ofxGVF.cpp
	emcc -DEMSCRIPTEN -O3 --memory-init-file 0 -I../src -std=c++11 GVFjs.cpp ../src/ofxGVF.cpp  --post-js GVF_wrapper.js -o js/GVF.min.js -s EXPORTED_FUNCTIONS=$(EXPORTED_FUNCTIONS)

all: GVF.js GVF.min.js