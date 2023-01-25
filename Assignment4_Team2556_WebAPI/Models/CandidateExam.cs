﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment4_Team2556_WebAPI.Models
{
    public class CandidateExam
    {
        public int CandidateExamId { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public int CandidateId { get; set; }
        public Candidate? Candidate { get; set; }
        public DateTime ExamDate { get; set; }
        public virtual ICollection<CandidateExamAnswer>? QA { get; set; }
        public string? AssessmentTestCode { get; set; }
        public DateTime? ExaminationDate { get; set; }
        public DateTime? ScoreReportDate { get; set; }
        public int? ExamScore { get; set;}
        public string? PercentageScore { get; set; }
        public string? TestResult { get; set; }
        public int? NumberOfAwardedMarks { get; set; }
        public int? NumberOfPossibleMakrs { get; set; }

        public CandidateExam()
        {

        }
    }
}