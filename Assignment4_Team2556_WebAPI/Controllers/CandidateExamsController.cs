﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment4_Team2556_WebAPI.Data;
using Assignment4_Team2556_WebAPI.Models;
using Assignment4_Team2556_WebAPI.Services;
using Assignment4_Team2556_WebAPI.Models.DTOModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace Assignment4_Team2556_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateExamsController : ControllerBase
    {
        private readonly ICandidateExamService _candidateExamService;
        private readonly ICandidateExamAnswerService _candidateExamAnswerService;
        private readonly IGenericService<Voucher> _voucherService;
        private readonly UserManager<User> _userManager;

        public CandidateExamsController(ICandidateExamService candidateExamService, ICandidateExamAnswerService candidateExamAnswerService, IGenericService<Voucher> voucherService, UserManager<User> userManager)
        {
            _candidateExamService = candidateExamService;
            _candidateExamAnswerService = candidateExamAnswerService;
            _voucherService = voucherService;
            _userManager = userManager;
        }


        //
        // GET: api/CandidateExams/List
        //
        // Summary: Get list of all candidate exams
        [HttpGet("List")]
        [Authorize(Roles = "Admin,Marker")]
        public async Task<IList<CandidateExam>> GetAllCandidateExams()
        {
            return await _candidateExamService.GetAllCandidateExams();
        }


        //
        // GET: api/CandidateExams/Unmarked
        //
        // Summary: Get list of all Unmarked candidate exams
        [HttpGet("Unmarked")]
        [Authorize(Roles = "Admin,Marker")]
        public async Task<IList<CandidateExam>> GetAllUnmarkedCandidateExams()
        {
            return await _candidateExamService.GetAllUnmarkedCandidateExams();
        }


        //
        // GET: api/CandidateExams
        //
        // Summary: Get List of all active certificates
        [HttpGet]
        public async Task<IList<Certificate>> GetCandidateExams()
        {
            return await _candidateExamService.GetActiveCertificateList();
        }


        // GET: api/CandidateExams/Certificates
        // Gets all the passed exams of a candidate
        [HttpGet("Certificates")]
        public async Task<IList<CandidateExam>> GetCandidatePassedExams(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            return await _candidateExamService.GetAccomplishedExamsByCandidateId(user.Id);
        }

        //
        // GET: api/CandidateExams/ScheduledExams
        [HttpGet("ScheduledExams")]
        public async Task<IList<CandidateExam>> GetScheduledExamsOfCandidate(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            return await _candidateExamService.GetScheduledExamsByCandidateId(user.Id);
        }


        //
        // GET: api/CandidateExams
        //
        // Summary: Get a candidate exam by id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Marker")]
        public async Task<IList<CandidateExam>> GetCandidateExam(int id)
        {
            return await _candidateExamService.GetCandidateExam(id);
        }


        //
        // GET: api/MarkedExams/{Marker's username}
        //
        // Summary: Get list of all marked exams associated with a particular marker
        [HttpGet("MarkedExams/{username}")]
        [Authorize(Roles = "Admin,Marker")]
        public async Task<IList<CandidateExam>> GetMarkedCandidateExamsByMarker(string username)
        {
            User marker = await _userManager.FindByNameAsync(username);
            return await _candidateExamService.GetAllMarkedCandidateExamsByMarker(marker.Id);
        }

        //
        // GET: api/UnmarkedExams//{Marker's username}
        //
        // Summary: Get list of unmarked exams that have been assigned to a particular marker
        [HttpGet("unmarkedexams/{username}")]
        [Authorize(Roles = "Admin,Marker")]
        public async Task<IList<CandidateExam>> GetUnMarkedCandidateExamsByMarker(string username)
        {
            User marker = await _userManager.FindByNameAsync(username);
            return await _candidateExamService.GetAllUnMarkedCandidateExamsByMarker(marker.Id);
        }


        //// GET: api/CandidateExams/ExamResults/5
        [HttpGet("ExamResults/{id}")]
        [Authorize(Roles = "Candidate,Admin")]
        public async Task<ActionResult<CandidateExamResultsDTO>> GetCandidateExamResults(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CandidateExamResultsDTO candidateExamResults = await _candidateExamService.GetMarksOfTheSubmitedExam(id);

            return Ok(candidateExamResults);
        }

        // GET: api/CandidateExams/GetScheduledExamForm
        [HttpGet("GetScheduledExamForm/{id}")]
        public async Task<ActionResult<ExamForm>> GetScheduledCandidateExam(int id)
        {
            CandidateExam candidateExam = await _candidateExamService.GetScheduledExamById(id);
            var examForm = await _candidateExamService.GetScheduledCandidateExamForm(candidateExam);
            return Ok(examForm);
        }

        // This controller checks if the voucher belongs to the user and fetches the exam
        //// GET: api/CandidateExams/InsertVoucher
        [HttpPost("InsertVoucher/{username}")]
        public async Task<ActionResult<ExamDetailsDTO>> GetCandidateExamWithVoucher(string userName, DateTime? examDate, Voucher voucher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userManager.FindByNameAsync(userName);

            if(voucher.CandidateId != user.Id && voucher.IsClaimed == true)
            {
                return BadRequest("The voucher is not valid! ");
            }

            if(examDate < DateTime.Now.Date)
            {
                return BadRequest("Invalid Date");
            }


            ExamForm examForm;

            if (examDate == null)
            {
                examForm = await _candidateExamService.GenerateExamForm(user.Id, voucher.CertificateId);
            }
            else
            {
                examForm = await _candidateExamService.GenerateExamForm(user.Id, voucher.CertificateId, examDate);
            }


            return Ok(examForm);
        }

        // PUT: api/CandidateExams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Marker")]
        public async Task<IActionResult> PutCandidateExam(int id, CandidateExam candidateExam)
        {
            if (id != candidateExam.CandidateExamId)
            {
                return BadRequest();
            }

            try
            {
                await _candidateExamService.AddSaveChanges(candidateExam);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidateExamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/CandidateExams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<ActionResult<ExamForm>> PostCandidateExam(ExamForm examForm)
        {
            if (!ModelState.IsValid) // Validate the exam form
            {
                return BadRequest(ModelState);
            }

            if (examForm.ChosenOptionsId == null) //Handler if exam is submitted blank
            {
                return Ok();
            }

            await _candidateExamAnswerService.SaveExamAnswers(examForm); //Process and save submitted answers

            return Ok(examForm);// Redirect to the Home page
        }

        //// DELETE: api/CandidateExams/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCandidateExam(int id)
        //{
        //    var candidateExam = await _context.CandidateExams.FindAsync(id);
        //    if (candidateExam == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CandidateExams.Remove(candidateExam);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CandidateExamExists(int id)
        {
            //return _context.CandidateExams.Any(e => e.CandidateExamId == id);
            if (_candidateExamService.GetCandidateExam(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
