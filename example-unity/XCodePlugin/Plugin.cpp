/*
	This is a simple plugin, a bunch of functions that do simple things.
*/

#include "Plugin.pch"
#include "ofxGVF.h"

ofxGVF              gvf;
ofxGVFConfig        config;
ofxGVFGesture       currentGesture;
ofxGVFOutcomes      outcomes;

std::vector<float> observation;

void setDimensions(int dim)
{
    config.inputDimensions = dim;
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

void addObservation(float *data, int numItems)
{
    std::vector<float> vecData(data, data+numItems);
    currentGesture.addObservation(vecData);
}

int getNumObservations(int gestureNum)
{
    if(gestureNum > 0 && gestureNum < gvf.getNumberOfGestureTemplates())
        return gvf.getGestureTemplate(gestureNum).getTemplateLength();
    else
        return -1;
}
void getObservation(int gestureNum, int observationNum, float *data)
{
    std::vector <float> vecData = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
    for (int i = 0; i < vecData.size(); i++)
    {
        data[i] = vecData[i];
    }
}
void getObservationZeroOrigin(int gestureNum, int observationNum, float *data)
{
    std::vector <float> vecData = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
    std::vector <float> firstObs = gvf.getGestureTemplate(gestureNum).getTemplate()[0];
    for (int i = 0; i < vecData.size(); i++)
    {
        data[i] = vecData[i] - firstObs[i];
    }
}

int  getNumObservationsCurrentGestures()
{
    return currentGesture.getTemplateLength();
}

void getObservationCurrentGestures(int observationNum, float *data)
{
    std::vector <float> vecData = currentGesture.getTemplate()[observationNum];
    for (int i = 0; i < vecData.size(); i++)
    {
        data[i] = vecData[i];
    }
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
    std::cout << "---------------------------------\n";
    std::cout << "get phase " << i << " " << outcomes.estimations.size() << std::endl;
    std::cout << "---------------------------------\n";
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
    std::cout << "=================================\n";
    std::cout << "ending learning\n";
    gvf.addGestureTemplate(currentGesture);
    currentGesture.clear();
}

void setGestureToCurrent(int index)
{
    std::cout << "=================================\n";
    std::cout << "setting gesture\n";
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







