
#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

// ------------------------------------------------------------------------
// Plugin itself

#include "ofxGVF.h"

ofxGVF              gvf;
ofxGVFConfig        config;
ofxGVFGesture       currentGesture;
ofxGVFOutcomes      outcomes;
ofxGVFParameters	defaultParameters;

bool parametersSet = false;

std::vector<float> observation;



// Link following functions C-style (required for plugins)
extern "C"
{

void initParameters()
{
	if(parametersSet) return;

	    // default parameters
    defaultParameters.numberParticles = 2000;
    defaultParameters.tolerance = 0.1f;
    defaultParameters.resamplingThreshold = 500;
    defaultParameters.distribution = 0.0f;
    defaultParameters.phaseVariance = 0.000001;
    defaultParameters.speedVariance = 0.001;
    defaultParameters.scaleVariance = vector<float>(1, 0.00001); // TODO: Check that default works like this.
    defaultParameters.rotationVariance = vector<float>(1, 0.00000);

	parametersSet = true;
}

void EXPORT_API  setDimensions(int dim)
{
    config.inputDimensions = dim;
}


void  EXPORT_API  setSegmentation(bool segmentation)
{
    config.segmentation = segmentation;
}

void  EXPORT_API  setMultiPoint(bool multipoint)
{
    config.multipoint = multipoint;
}



void  EXPORT_API  setRotationFeatures(bool rotationFeatures)
{
    config.rotationFeatures = rotationFeatures;
}


void EXPORT_API  initGVF()
{
    // CONFIGURATION of the GVF
    //config.inputDimensions = 2;
    config.translate       = true;
    //config.segmentation    = false;
    
    // PARAMETERS are set by default
	initParameters();
    
    // CREATE the corresponding GVF
    gvf.setup(config, defaultParameters);
    
    //std::cout << "did the config" << std::endl;
}


void EXPORT_API  addObservation(float *data, int numItems)
{
    std::vector<float> vecData(data, data+numItems);
    currentGesture.addObservation(vecData);
}


int EXPORT_API  getNumObservations(int gestureNum)
{
    if(gestureNum >= 0 && gestureNum < gvf.getNumberOfGestureTemplates())
        return gvf.getGestureTemplate(gestureNum).getTemplateLength();
    else
        return -1;
}
void EXPORT_API  getObservation(int gestureNum, int observationNum, float *data)
{
    std::vector <float> vecData = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
    for (int i = 0; i < vecData.size(); i++)
    {
        data[i] = vecData[i];
    }
}
void EXPORT_API  getObservationZeroOrigin(int gestureNum, int observationNum, float *data)
{
    std::vector <float> vecData = gvf.getGestureTemplate(gestureNum).getTemplate()[observationNum];
    std::vector <float> firstObs = gvf.getGestureTemplate(gestureNum).getTemplate()[0];
    for (int i = 0; i < vecData.size(); i++)
    {
        data[i] = vecData[i] - firstObs[i];
    }
}

int  EXPORT_API   getNumObservationsCurrentGestures()
{
    return currentGesture.getTemplateLength();
}

void  EXPORT_API   getObservationCurrentGestures(int observationNum, float *data)
{
    std::vector <float> vecData = currentGesture.getTemplate()[observationNum];
    for (int i = 0; i < vecData.size(); i++)
    {
        data[i] = vecData[i];
    }
}


void EXPORT_API  infer()
{
    gvf.infer(currentGesture.getLastRawObservation());
    outcomes = gvf.getOutcomes();
}

int  EXPORT_API  getNumberOfGestureTemplates()
{
    return gvf.getNumberOfGestureTemplates();
}

int  EXPORT_API  getMostProbable()
{
    return outcomes.most_probable;
}


float  EXPORT_API  getProbability(int i)
{
    return outcomes.estimations[i].probability;
}

float EXPORT_API  getPhase(int i)
{
    return outcomes.estimations[i].phase;
}

float EXPORT_API  getSpeed(int i)
{
    return outcomes.estimations[i].speed;
}

float EXPORT_API  getScale(int i)
{
    return outcomes.estimations[i].scale[0];
}

float EXPORT_API  getRotation(int i)
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


void EXPORT_API  startLearning() 
{
    gvf.setState(ofxGVF::STATE_LEARNING);
    currentGesture.clear();
    currentGesture.setAutoAdjustRanges(true);
    //currentGesture.setMin(0.0f, 0.0f);
    //currentGesture.setMax(ofGetWidth(), ofGetHeight());
    //currentGesture.addObservationRaw(ofPoint(x, y, 0));
}

void  EXPORT_API  endLearning()
{
    gvf.addGestureTemplate(currentGesture);
    currentGesture.clear();
}

void EXPORT_API setGestureToCurrent(int index)
{
    std::cout << "=================================\n";
    std::cout << "setting gesture\n";
    gvf.addGestureTemplate(currentGesture, index);
    currentGesture.clear();
}



bool  EXPORT_API  isLearning()
{
    return (gvf.getState() == ofxGVF::STATE_LEARNING);
}

void  EXPORT_API  startFollowing()
{
    gvf.setState(ofxGVF::STATE_FOLLOWING);
    gvf.spreadParticles();
    currentGesture.clear();
    currentGesture.setAutoAdjustRanges(false);
}

void  EXPORT_API  endFollowing()
{
    currentGesture.clear();
    gvf.spreadParticles();
}


bool  EXPORT_API  isFollowing()
{
    return (gvf.getState() == ofxGVF::STATE_FOLLOWING);
}


void  EXPORT_API setTolerance(float v)
{
	initParameters();
    defaultParameters.tolerance = v;
}

float  EXPORT_API  getTolerance()
{
    return gvf.getTolerance();
}
void  EXPORT_API  setScaleVariance(float v)
{
   initParameters();
   defaultParameters.scaleVariance = vector<float>(1, v);
}
float  EXPORT_API getScaleVariance()
{
    return gvf.getScaleVariance()[0];
}
void  EXPORT_API  setSpeedVariance(float v)
{
   initParameters();
   defaultParameters.speedVariance = v;
}
float  EXPORT_API getSpeedVariance()
{
    return gvf.getSpeedVariance();
}

void  EXPORT_API  setNumberOfParticles(int n)
{
   initParameters();
   defaultParameters.numberParticles = n;
}
int  EXPORT_API getNumberOfParticles()
{
    return gvf.getNumberOfParticles();
}


void  EXPORT_API saveTemplates(const char * filename)
{
    gvf.saveTemplates(filename);
}
void  EXPORT_API loadTemplates(const char * filename)
{
    gvf.loadTemplates(filename);
}


void  EXPORT_API clear()
{
    gvf.clear();
}



} // end of export C block
