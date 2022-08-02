using Dragons_Breath.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Data
{
    public enum Roles
    {
        Admin,
        ProjectManager,
        Developer,
        Submitter,
        NewUser,
        Demo
    }

    public static class ContextSeed
    {
        public static async Task RunSeedMethodsAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<BTUser> userManager,
            ApplicationDbContext context)
        {
            await SeedRolesAsync(roleManager);
            await SeedDefaultUsersAsync(userManager);
            await SeedTicketTypesAsync(context);
            await SeedTicketStatusesAsync(context);
            await SeedTicketPrioritiesAsync(context);
            await SeedProjectsAsync(context);
            await SeedProjectUsersAsync(context, userManager);
            await SeedTicketsAsync(context, userManager);
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Demo.ToString()));
        }
        private static async Task SeedDefaultUsersAsync(UserManager<BTUser> userManager)
        {

            #region Admin Seed
            var defaultUser = new BTUser
            {
                UserName = "arussell@mailinator.com",
                Email = "arussell@mailinator.com",
                FirstName = "Drew",
                LastName = "Russell",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Project Manager Seed
            defaultUser = new BTUser
            {
                UserName = "bobtester@mailinator.com",
                Email = "bobtester@mailinator.com",
                FirstName = "Bob",
                LastName = "Tester",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Project Manager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Developer Seed
            defaultUser = new BTUser
            {
                UserName = "marytester@mailinator.com",
                Email = "marytester@mailinator.com",
                FirstName = "Mary",
                LastName = "Tester",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Submitter Seed
            defaultUser = new BTUser
            {
                UserName = "lucindatester@mailinator.com",
                Email = "lucindatester@mailinator.com",
                FirstName = "Lucinda",
                LastName = "Tester",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region New User Seed
            defaultUser = new BTUser
            {
                UserName = "georgetester@mailinator.com",
                Email = "georgetester@mailinator.com",
                FirstName = "George",
                LastName = "Tester",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion

            string demoPassword = "Xyz%987$";
            //These are my seeded demo users for showing off the software
            //Each user occupies a "main" role and the new Demo role
            //We will target this Demo role to prevent demo users from changing the database
            #region Demo Admin Seed
            defaultUser = new BTUser
            {
                UserName = "demoadmin@mailinator.com",
                Email = "demoadmin@mailinator.com",
                FirstName = "Andrew",
                LastName = "Roussel",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo Project Manager Seed
            defaultUser = new BTUser
            {
                UserName = "demopm@mailinator.com",
                Email = "demopm@mailinator.com",
                FirstName = "Bobby",
                LastName = "Testero",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Project Manager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo Developer Seed
            defaultUser = new BTUser
            {
                UserName = "marianatester@mailinator.com",
                Email = "marianatester@mailinator.com",
                FirstName = "Mariana",
                LastName = "Testero",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo Submitter Seed
            defaultUser = new BTUser
            {
                UserName = "lucytester@mailinator.com",
                Email = "lucytester@mailinator.com",
                FirstName = "Lucy",
                LastName = "Testero",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo New User Seed
            defaultUser = new BTUser
            {
                UserName = "georgietester@mailinator.com",
                Email = "georgietester@mailinator.com",
                FirstName = "Georgie",
                LastName = "Testero",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion

            //Add in random generated users
        }
        private static async Task SeedTicketTypesAsync(ApplicationDbContext context)
        {
            foreach(var type in context.TicketTypes.ToList())
            {
                context.TicketTypes.Remove(type);
                await context.SaveChangesAsync();

            }
            #region Seed defaultSeedUI
            // Seed UI TicketType
            var defaultSeedUI = new TicketType
            {
                Name = "User Interface"
            };
            try
            {
                var type = await context.TicketTypes.Where(tt => tt.Name == "UI").FirstOrDefaultAsync();
                if (type == null)
                {
                    context.TicketTypes.Add(defaultSeedUI);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Default UI Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            #endregion
            var type1 = new TicketType
            {
                Name = "User Experience"
            };
            var type2 = new TicketType
            {
                Name = "Site Crashes"
            };
            var type3 = new TicketType
            {
                Name = "Database Issue"
            };
            var type4 = new TicketType
            {
                Name = "Unknown"
            };
            context.TicketTypes.Add(type1);
            context.TicketTypes.Add(type2);
            context.TicketTypes.Add(type3);
            context.TicketTypes.Add(type4);
            context.SaveChanges();

        }
        private static async Task SeedTicketStatusesAsync(ApplicationDbContext context)
        {
            foreach (var status in context.TicketStatuses.ToList())
            {
                context.TicketStatuses.Remove(status);
                await context.SaveChangesAsync();

            }

            #region Seed defaultSeedUI
            // Seed UI TicketType
            var defaultSeedUI = new TicketStatus
            {
                Name = "On Hold"
            };
            try
            {
                var type = await context.TicketStatuses.Where(tt => tt.Name == "UI").FirstOrDefaultAsync();
                if (type == null)
                {
                    await context.TicketStatuses.AddAsync(defaultSeedUI);
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Default UI Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            #endregion
            if (context.TicketStatuses.Count() == 1)
            {
                var status1 = new TicketStatus
                {
                    Name = "Unassigned"
                };
                var status2 = new TicketStatus
                {
                    Name = "In Progress"
                };
                var status3 = new TicketStatus
                {
                    Name = "Resolved"
                };
                var status4 = new TicketStatus
                {
                    Name = "More Info Needed"
                };
                context.TicketStatuses.Add(status1);
                context.TicketStatuses.Add(status2);
                context.TicketStatuses.Add(status3);
                context.TicketStatuses.Add(status4);
                context.SaveChanges();
            }
        }
        private static async Task SeedTicketPrioritiesAsync(ApplicationDbContext context)
        {
            foreach (var priority in context.TicketPriorities.ToList())
            {
                context.TicketPriorities.Remove(priority);
                await context.SaveChangesAsync();

            }


            #region Seed defaultSeedUI
            // Seed UI TicketType
            var defaultSeedUI = new TicketPriority
            {
                Name = "On Hold"
            };
            try
            {
                var type = await context.TicketPriorities.Where(tt => tt.Name == "UI").FirstOrDefaultAsync();
                if (type == null)
                {
                    await context.TicketPriorities.AddAsync(defaultSeedUI);
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Default UI Ticket Type.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            #endregion
            if (context.TicketPriorities.Count() == 1)
            {
                var prior1 = new TicketPriority
                {
                    Name = "Low"
                };
                var prior2 = new TicketPriority
                {
                    Name = "Normal"
                }; var prior3 = new TicketPriority
                {
                    Name = "High"
                };
                var prior4 = new TicketPriority
                {
                    Name = "Critical"
                };
                context.TicketPriorities.Add(prior1);
                context.TicketPriorities.Add(prior2);
                context.TicketPriorities.Add(prior3);
                context.TicketPriorities.Add(prior4);
                context.SaveChanges();
            }

        }
        private static async Task SeedProjectsAsync(ApplicationDbContext context)
        {
            foreach (var project in context.Projects.ToList())
            {
                context.Projects.Remove(project);
                await context.SaveChangesAsync();

            }

            Project seedProject1 = new Project
            {
                Name = "Blog Project"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog Project");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject1);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Default Project 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            Project seedProject2 = new Project
            {
                Name = "Bug Tracker Project"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker Project");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject2);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Default Project 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            Project seedProject3 = new Project
            {
                Name = "Financial Portal Project"
            };
            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal Project");
                if (project == null)
                {
                    await context.Projects.AddAsync(seedProject3);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Default Project 3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

        }
        private static async Task SeedProjectUsersAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("demoadmin@mailinator.com")).Id;
            string projectManagerId = (await userManager.FindByEmailAsync("demopm@mailinator.com")).Id;
            string developerId = (await userManager.FindByEmailAsync("marianatester@mailinator.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("lucytester@mailinator.com")).Id;
            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog Project")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker Project")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal Project")).Id;
            ProjectUser projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Admin Project 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Admin Project 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Admin Project 3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding PM Project 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding PM Project 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding PM Project 3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };
        }
        private static async Task SeedTicketsAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            foreach (var notification in context.Notifications.ToList())
            {
                context.Notifications.Remove(notification);
                await context.SaveChangesAsync();
            }


            string developerId = (await userManager.FindByEmailAsync("marianatester@mailinator.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("lucytester@mailinator.com")).Id;
            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog Project")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Bug Tracker Project")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal Project")).Id;
            int statusId = (await context.TicketStatuses.FirstOrDefaultAsync(ts => ts.Name == "In Progress")).Id;
            int typeId = (await context.TicketTypes.FirstOrDefaultAsync(tt => tt.Name == "User Experience")).Id;
            int priorityId = (await context.TicketPriorities.FirstOrDefaultAsync(tp => tp.Name == "Critical")).Id;

            Ticket ticket = new Ticket
            {
                Title = "Need more blog posts",
                Description = "It's not a real blog when you only have a single post. Our users have requested you present more content. Without the content the Google crawlers will never up our organic ranking",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project1Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Need more blog posts");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Ticket 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

        }
    }
}