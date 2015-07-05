/*
	An interface between javascript and GVF
*/

#include "GVFjs.h"
#include "ofxGVF.h"


ofxGVF              gvf;
ofxGVFConfig        config;
ofxGVFGesture       currentGesture;
ofxGVFOutcomes      outcomes;

std::vector<float> observation;
std::vector<float> firstObservation;


void setDimensions(int dim)
{
    config.inputDimensions = dim;
    printf("dimensions %d", dim);
}


void setSegmentation(bool segmentation)
{
    config.segmentation = segmentation;
}

void setMultiPoint(bool multipoint)
{
    config.multipoint = multipoint;
}



void setRotationFeatures(bool rotationFeatures)
{
    config.rotationFeatures = rotationFeatures;
}


void initGVF()
{
    // CONFIGURATION of the GVF
    //config.inputDimensions = 2;
    config.translate       = true;
    //config.segmentation    = false;
    
    // PARAMETERS are set by default
    
    // CREATE the corresponding GVF
    gvf.setup(config);
    
    //std::cout << "did the config" << std::endl;
}

void addObservation2D(float x, float y)
{
    std::vector<float> vecData;
    vecData.push_back(x);
    vecData.push_back(y);
    currentGesture.addObservation(vecData);
}

int getNumObservations(int gestureNum)
{
    if(gestureNum >= 0 && gestureNum < gvf.getNumberOfGestureTemplates())
        return gvf.getGestureTemplate(gestureNum).getTemplateLength();
    else
        return -1;
}
void getObservation(int gestureNum, int observationNum)
{
	observation = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
    //firstObservation = gvf.getGestureTemplate(gestureNum).getTemplate()[0];
    // std::vector <float> vecData = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
    // for (int i = 0; i < vecData.size(); i++)
    // {
    //     data[i] = vecData[i];
    // }
}

float getObservationVal(int i)
{
	if(i >= 0 && i < observation.size()){
		return observation[i];
	} else {
		return 0.0f;
	}
}
/*
float getObservationValZeroOrigin(int i)
{
	if(i > 0 && i < observation.size()){
		if(i < firstObservation.size()){
				return observation[i] - firstObservation[i];
		} else {
			return observation[i];
		}
	} else {
		return 0.0f;
	}
}
*/

// void getObservationZeroOrigin(int gestureNum, int observationNum, float *data)
// {
//     std::vector <float> vecData = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
//     std::vector <float> firstObs = gvf.getGestureTemplate(gestureNum).getTemplate()[0];
//     for (int i = 0; i < vecData.size(); i++)
//     {
//         data[i] = vecData[i] - firstObs[i];
//     }
// }

int  getNumObservationsCurrentGestures()
{
    return currentGesture.getTemplateLength();
}

void getObservationCurrentGestures(int observationNum)
{
	observation = currentGesture.getTemplate()[observationNum];
	//firstObservation = currentGesture.getTemplate()[0];
    // std::vector <float> vecData = currentGesture.getTemplate()[observationNum];
    // for (int i = 0; i < vecData.size(); i++)
    // {
    //     data[i] = vecData[i];
    // }
}


void infer()
{
    gvf.infer(currentGesture.getLastRawObservation());
    //std::cout << "--------------------------\n";
    //std::cout << "done inference, getting outcomes\n";
    //std::cout << "--------------------------\n";
    outcomes = gvf.getOutcomes();
    //std::cout << "estimations length " << outcomes.estimations.size() << std::endl;
}

int getNumberOfGestureTemplates()
{
    return gvf.getNumberOfGestureTemplates();
}

int getMostProbable()
{
    return outcomes.most_probable;
}


float getProbability(int i)
{
    return outcomes.estimations[i].probability;
}

float getPhase(int i)
{
    //std::cout << "---------------------------------\n";
    //std::cout << "get phase " << i << " " << outcomes.estimations.size() << std::endl;
    //std::cout << "---------------------------------\n";
    return outcomes.estimations[i].phase;
}

float getSpeed(int i)
{
    return outcomes.estimations[i].speed;
}

float getScale(int i)
{
    return outcomes.estimations[i].scale[0];
}

float getRotation(int i)
{
    return outcomes.estimations[i].rotation[0];
}
//
//void getScale(float *s)
//{
//    for (int i = 0; i < 3; i++)
//        s[i] = outcomes.estimations[outcomes.most_probable].scale[i];
//}
//
//void getRotation(float *r)
//{
//    for (int i = 0; i < 3; i++)
//        r[i] = outcomes.estimations[outcomes.most_probable].rotation[i];
//}


void startLearning()
{
    gvf.setState(ofxGVF::STATE_LEARNING);
    currentGesture.clear();
    currentGesture.setAutoAdjustRanges(true);
    //currentGesture.setMin(0.0f, 0.0f);
    //currentGesture.setMax(ofGetWidth(), ofGetHeight());
    //currentGesture.addObservationRaw(ofPoint(x, y, 0));
}

void endLearning()
{
    //std::cout << "=================================\n";
    //std::cout << "ending learning\n";
    gvf.addGestureTemplate(currentGesture);
    currentGesture.clear();
}

void setGestureToCurrent(int index)
{
    //std::cout << "=================================\n";
    //std::cout << "setting gesture\n";
    gvf.addGestureTemplate(currentGesture, index);
    currentGesture.clear();
}


bool isLearning()
{
    return (gvf.getState() == ofxGVF::STATE_LEARNING);
}

void startFollowing()
{
    gvf.setState(ofxGVF::STATE_FOLLOWING);
    gvf.spreadParticles();
    currentGesture.clear();
    currentGesture.setAutoAdjustRanges(false);
}

void endFollowing()
{
    //gvf.setState(ofxGVF::STATE_CLEAR);
    currentGesture.clear();
    gvf.spreadParticles();
}


bool isFollowing()
{
    return (gvf.getState() == ofxGVF::STATE_FOLLOWING);
}


void  setTolerance(float v)
{
    gvf.setTolerance(v);
}
float getTolerance()
{
    return gvf.getTolerance();
}
void  setPhaseVariance(float v)
{
    gvf.setPhaseVariance(v);
}
float getPhaseVariance()
{
    return gvf.getPhaseVariance();
}
void  setScaleVariance(float v)
{
    gvf.setScaleVariance(v);
}
float getScaleVariance()
{
    return gvf.getScaleVariance()[0];
}
void  setSpeedVariance(float v)
{
    gvf.setSpeedVariance(v);
}
float getSpeedVariance()
{
    return gvf.getSpeedVariance();
}
void  setRotationVariance(float v)
{
    gvf.setRotationVariance(v);
}
float getRotationVariance()
{
    return gvf.getRotationVariance()[0];
}


void saveTemplates(const char * filename)
{
    gvf.saveTemplates(filename);
}
void loadTemplates(const char * filename)
{
    gvf.loadTemplates(filename);
}

void clear()
{
    gvf.clear();
}






