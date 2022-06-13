using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class DoctorSurveyResult
    {
        private DoctorUser _doctor;
        private int _surveyCount;
        private float _averageQuality;
        private int[] _qualityCount;
        private float _averageRecommendation;
        private int[] _recommendationCount;
        private List<string> _comments;

        public DoctorUser Doctor { get { return _doctor; } }
        public int SurveyCount { get { return _surveyCount; } }
        public float AverageQuality { get { return _averageQuality; } }
        public int[] QualityCount { get { return _qualityCount; } }
        public float AverageRecommendation { get { return _averageRecommendation; } }
        public int[] RecommendationCount { get { return _recommendationCount; } }
        public List<string> Comments { get { return _comments; } }

        public DoctorSurveyResult(DoctorUser doctor, int surveyCount, float averageQuality, int[] qualityCount, 
            float averageRecommendation, int[] recommendationCount, List<string> comments)
        {
            _doctor = doctor;
            _surveyCount = surveyCount;
            _averageQuality = averageQuality;
            _qualityCount = qualityCount;
            _averageRecommendation = averageRecommendation;
            _recommendationCount = recommendationCount;
            _comments = comments;
        }
    }
}
