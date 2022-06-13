using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Users.Model
{
    public class HospitalSurveyResult
    {
        private int _surveyCount;
        private float _averageQuality;
        private int[] _qualityCount;
        private float _averageCleanliness;
        private int[] _cleanlinessCount;
        private float _averageSatisfied;
        private int[] _satisfiedCount;
        private float _averageRecommendation;
        private int[] _recommendationCount;
        private List<string> _comments;

        public int SurveyCount { get { return _surveyCount; } }
        public float AverageQuality { get { return _averageQuality; } }
        public int[] QualityCount { get { return _qualityCount; } }
        public float AverageCleanliness { get { return _averageCleanliness; } }
        public int[] CleanlinessCount { get { return _cleanlinessCount; } }
        public float AverageSatisfied { get { return _averageSatisfied; } }
        public int[] SatisfiedCount { get { return _satisfiedCount; } }
        public float AverageRecommendation { get { return _averageRecommendation; } }
        public int[] RecommendationCount { get { return _recommendationCount; } }
        public List<string> Comments { get { return _comments; } }

        public HospitalSurveyResult(int surveyCount, float averageQuality, int[] qualityCount, float averageCleanliness, int[] cleanlinessCount, 
            float averageSatisfied, int[] satisfiedCount, float averageRecommendation, int[] recommendationCount, List<string> comments)
        {
            _surveyCount = surveyCount;
            _averageQuality = averageQuality;
            _qualityCount = qualityCount;
            _averageCleanliness = averageCleanliness;
            _cleanlinessCount = cleanlinessCount;
            _averageSatisfied = averageSatisfied;
            _satisfiedCount = satisfiedCount;
            _averageRecommendation = averageRecommendation;
            _recommendationCount = recommendationCount;
            _comments = comments;
        }
    }
}
