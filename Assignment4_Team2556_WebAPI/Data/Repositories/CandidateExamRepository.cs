﻿using Assignment4_Team2556_WebAPI.Models;
using Assignment4_Team2556_WebAPI.Data;
using Assignment4_Team2556_WebAPI.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using NuGet.Protocol.Plugins;
using Assignment4_Team2556_WebAPI.Data.Repositories;

namespace Assignment4_Team2556_WebAPI.Data.Repositories
{
    public class CandidateExamRepository : ICandidateExamRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidateExamRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        //
        //Summary: Returns a List of Exams, associated with a certificate
        public async Task<IList<Exam>> GetAllExamsByCertificateId(int certificateId)
        {
            return await _context.Exams.Where(i => i.CertificateId == certificateId).ToListAsync();

        }

        //
        //Summary: Returns a List of all active certificates
        public async Task<IList<Certificate>> GetActiveCertificateList() 
        {
            return await _context.Certificates.Where(v => v.IsActive == true).ToListAsync();
        }

        //
        //Summary: Returns a List of all candidate exams
        public async Task<IList<CandidateExam>> GetAllCandidateExams()
        {
            return await _context.CandidateExams
                .Include(m => m.Marker)
                .Include(c => c.Candidate)
                .Include(e => e.Exam).ThenInclude(c => c.Certificate)
                .ToListAsync();
        }

        //
        //Summary: Returns a Candidate Exam that has been submited for marking 
        public async Task<CandidateExam> GetSubmitedCandidateExamById(int id)
        {
            var candidateExam = await _context.CandidateExams.Where(c => c.CandidateExamId == id).FirstAsync();
            await _context.Entry(candidateExam).Reference(x => x.Exam).LoadAsync();
            return candidateExam;   
        }

        //
        //Summary: Returns a List of ExamQuestions, associated with a certain Exam
        public async Task<IList<ExamQuestion>> GetAllExamQuestionsByExamId(int examId)
        {
            return await _context.ExamQuestions.Where(exam => exam.ExamId == examId).Include(eq => eq.Question.Options).ToListAsync();
        }

        //
        //Summary: Saves a Candidate Exam to the database
        public async Task AddSaveChanges(CandidateExam candidateExam)
        {
            // Update here Works As AddOrUpdate
            _context.Update(candidateExam);
            await _context.SaveChangesAsync();
        }
    }
}
