
#ifdef EMSCRIPTEN
    //#define EXPORT_DECORATION EMSCRIPTEN_KEEPALIVE
    #define EXPORT_DECORATION 
#else
    #define EXPORT_DECORATION
#endif



extern "C" {

  
    void EXPORT_DECORATION setDimensions(int dim);
    void EXPORT_DECORATION setSegmentation(bool segmentation);
    void EXPORT_DECORATION setMultiPoint(bool multipoint);
    void EXPORT_DECORATION setRotationFeatures(bool rotationFeatures);
    
    void EXPORT_DECORATION initGVF();
    
    void  EXPORT_DECORATION addObservation2D(float x, float y);
    int   EXPORT_DECORATION getNumObservations(int gestureNum);
    void  EXPORT_DECORATION getObservation(int gestureNum, int observationNum);
    float EXPORT_DECORATION getObservationVal(int i);
    //void EXPORT_DECORATION getObservationZeroOrigin(int gestureNum, int observationNum, float *data);
    
    int  EXPORT_DECORATION getNumObservationsCurrentGestures();
    void EXPORT_DECORATION getObservationCurrentGestures(int observationNum);
    
    void EXPORT_DECORATION infer();
    
    int EXPORT_DECORATION getNumberOfGestureTemplates();
    
    int   EXPORT_DECORATION getMostProbable();
    float EXPORT_DECORATION getProbability(int i);
    float EXPORT_DECORATION getPhase(int i);
    float EXPORT_DECORATION getSpeed(int i);
    float EXPORT_DECORATION getScale(int i);
    float EXPORT_DECORATION getRotation(int i);
//    void getScale(float *s);
//    void getRotation(float *r);
//    
    void EXPORT_DECORATION startLearning();
    void EXPORT_DECORATION endLearning();
    void EXPORT_DECORATION setGestureToCurrent(int index);
    bool EXPORT_DECORATION isLearning();
    void EXPORT_DECORATION startFollowing();
    void EXPORT_DECORATION endFollowing();
    bool EXPORT_DECORATION isFollowing();
    
    void  EXPORT_DECORATION setTolerance(float v);
    float EXPORT_DECORATION getTolerance();
    void  EXPORT_DECORATION setPhaseVariance(float v);
    float EXPORT_DECORATION getPhaseVariance();
    void  EXPORT_DECORATION setScaleVariance(float v);
    float EXPORT_DECORATION getScaleVariance();
    void  EXPORT_DECORATION setSpeedVariance(float v);
    float EXPORT_DECORATION getSpeedVariance();
    void  EXPORT_DECORATION setRotationVariance(float v);
    float EXPORT_DECORATION getRotationVariance();
    
    void EXPORT_DECORATION saveTemplates(const char * filename);
    void EXPORT_DECORATION loadTemplates(const char * filename);

    void EXPORT_DECORATION clear();    

}