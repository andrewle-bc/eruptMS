﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COMP4911Timesheets.Data;
using COMP4911Timesheets.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace COMP4911Timesheets.Controllers
{
    public class WeeklyWorkPackagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int? projectId;
        private static int? parentWorkPKId;
        public static int PROJECT_CODE_LENGTH = 4;
        private readonly UserManager<Employee> _userManager;

        public WeeklyWorkPackagesController(ApplicationDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WorkPackages
        public async Task<IActionResult> Index()
        {
            var workPackages = new List<WorkPackage>();

        
                workPackages = await _context.WorkPackages
                    .Include(w => w.ParentWorkPackage)
                    .Include(w => w.Project)
                    .Include(w => w.ProjectEmployees)
                    .ToListAsync();

            return View(workPackages);
        }

        // GET: WorkPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var workPackage = await _context.WorkPackages
                .Include(w => w.ParentWorkPackage)
                .Include(w => w.Project)
                .FirstOrDefaultAsync(m => m.WorkPackageId == id);

            var budgets = await _context.Budgets.Where(a => a.WorkPackageId == id).Include(a => a.PayGrade).ToListAsync();
            /*
            for (int i = 0; i < budgets.Count; i++) {
                budgets[i].PayGrade = await _context.PayGrades.FirstOrDefaultAsync(m => m.PayGradeId == budgets[i].PayGradeId);
            }
            */
            workPackage.Budgets = budgets;

            if (workPackage == null)
            {
                return NotFound();
            }

            return View(workPackage);
        }
    }
}