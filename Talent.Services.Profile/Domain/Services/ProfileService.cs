using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Services.Profile.Domain.Contracts;
using Talent.Services.Profile.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Talent.Services.Profile.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Talent.Common.Security;
using Microsoft.AspNetCore.Mvc;

namespace Talent.Services.Profile.Domain.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserAppContext _userAppContext;
        IRepository<UserLanguage> _userLanguageRepository;
        IRepository<UserSkill> _userSkillRepository;
        IRepository<UserExperience> _userExperienceRepository;
        IRepository<User> _userRepository;
        IRepository<Employer> _employerRepository;
        IRepository<Job> _jobRepository;
        IRepository<Recruiter> _recruiterRepository;
        IFileService _fileService;

        public ProfileService(IUserAppContext userAppContext,
                              IRepository<UserLanguage> userLanguageRepository,
                              IRepository<UserSkill> userSkillRepository,
                              IRepository<UserExperience> userExperienceRepository,
                              IRepository<User> userRepository,
                              IRepository<Employer> employerRepository,
                              IRepository<Job> jobRepository,
                              IRepository<Recruiter> recruiterRepository,
                              IFileService fileService
                              )
        {
            _userAppContext = userAppContext;
            _userLanguageRepository = userLanguageRepository;
            _userSkillRepository = userSkillRepository;
            _userExperienceRepository = userExperienceRepository;
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _jobRepository = jobRepository;
            _recruiterRepository = recruiterRepository;
            _fileService = fileService;
        }

        public bool AddNewLanguage(AddLanguageViewModel language,string id)
        {
            try
            {
                if (language.Id == null)
                {
                    var userLanguage = new UserLanguage
                    {
                        UserId = id,
                        Id = language.Id,
                        Language = language.Name,
                        LanguageLevel = language.Level
                    };
                    _userLanguageRepository.Add(userLanguage);
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateLanguage(AddLanguageViewModel language)
        {
            try
            {
                UserLanguage userLanguage = await _userLanguageRepository.GetByIdAsync(language.Id);
                userLanguage.Language = language.Name;
                userLanguage.LanguageLevel = language.Level;
                await _userLanguageRepository.Update(userLanguage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RemoveLanguage(AddLanguageViewModel language)
        {
            try
            {
                UserLanguage userLanguage = await _userLanguageRepository.GetByIdAsync(language.Id);
                userLanguage.IsDeleted = true;
                await _userLanguageRepository.Update(userLanguage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region Build Views from Model
        protected AddLanguageViewModel ViewModelFromLanguage(UserLanguage language)
        {
            return new AddLanguageViewModel
            {
                Name = language.Language,
                Level = language.LanguageLevel,
                Id = language.Id
            };
        }
        #endregion
        public bool AddNewSkill(AddSkillViewModel skill,string id)
        {
            try
            {
                if(skill.Id == null)
                {
                    var userSkill = new UserSkill
                    {
                        UserId = id,
                        Id = skill.Id,
                        Skill = skill.Name,
                        ExperienceLevel = skill.Level
                    };
                    _userSkillRepository.Add(userSkill);
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateSkill(AddSkillViewModel skill, string updaterId)
        {
            try
            {
                if (skill.Id != null)
                {
                    UserSkill userSkill = await _userSkillRepository.GetByIdAsync(skill.Id);
                    userSkill.Skill = skill.Name;
                    userSkill.ExperienceLevel = skill.Level;
                    await _userSkillRepository.Update(userSkill);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        public async Task<bool> RemoveSkill(AddSkillViewModel skill)
        {
            try
            {
                if (skill.Id != null)
                {
                    UserSkill userSkill = await _userSkillRepository.GetByIdAsync(skill.Id);
                    userSkill.IsDeleted = true;
                    await _userSkillRepository.Update(userSkill);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #region Build Views from Model
        protected AddSkillViewModel ViewModelFromSkill(UserSkill skill)
        {
            return new AddSkillViewModel
            {
                Id = skill.Id,
                Level = skill.ExperienceLevel,
                Name = skill.Skill
            };
        }
        #endregion
        public bool AddNewExperience(ExperienceViewModel experience, string id)
        {
            try
            {
                if (experience.Id == null)
                {
                    var userExperience = new UserExperience
                    {
                        UserId = id,
                        Company=experience.Company,
                        Position=experience.Position,
                        Responsibilities=experience.Responsibilities,
                        Start=experience.Start,
                        End=experience.End
                    };
                    _userExperienceRepository.Add(userExperience);
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateExperience(ExperienceViewModel experience)
        {
            try
            {
                UserExperience userExperience = await _userExperienceRepository.GetByIdAsync(experience.Id);
                userExperience.Company = experience.Company;
                userExperience.Position = experience.Position;
                userExperience.Responsibilities = experience.Responsibilities;
                userExperience.Start = experience.Start;
                userExperience.End = experience.End;
                await _userExperienceRepository.Update(userExperience);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RemoveExperience(ExperienceViewModel experience)
        {
            try
            {
                UserExperience userExperience = await _userExperienceRepository.GetByIdAsync(experience.Id);
                userExperience.IsDeleted = true;
                await _userExperienceRepository.Update(userExperience);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region Build Views from Model
        protected ExperienceViewModel ViewModelFromExperience(UserExperience experience)
        {
            return new ExperienceViewModel
            {
                Id = experience.Id,
                Company=experience.Company,
                Position=experience.Position,
                Responsibilities=experience.Responsibilities,
                Start=experience.Start,
                End=experience.End
            };
        }
        #endregion
        public async Task<TalentProfileViewModel> GetTalentProfile(string Id)
        {
            try
            {
                User Profile = null;
                Profile = await _userRepository.GetByIdAsync(Id);

                var languages = await _userLanguageRepository.Get(x => x.UserId == Profile.Id && x.IsDeleted == false);
                var currentUserLanguages = languages.Select(x => ViewModelFromLanguage(x)).ToList();

                var skill = await _userSkillRepository.Get(x => x.UserId == Profile.Id && x.IsDeleted == false);
                var currentUserSkill = skill.Select(x => ViewModelFromSkill(x)).ToList();

                var experience = await _userExperienceRepository.Get(x => x.UserId == Profile.Id && x.IsDeleted == false);
                var currentUserExperience = experience.Select(x => ViewModelFromExperience(x)).ToList();

                Profile.ProfilePhotoUrl = string.IsNullOrWhiteSpace(Profile.ProfilePhoto)
                          ? ""
                          : await _fileService.GetFileURL(Profile.ProfilePhoto, FileType.ProfilePhoto);


                var result = new TalentProfileViewModel
                {
                    Id = Profile.Id,
                    FirstName = Profile.FirstName,
                    LastName = Profile.LastName,
                    Phone = Profile.Phone,
                    Email = Profile.Email,
                    Description = Profile.Description,
                    Summary = Profile.Summary,
                    LinkedAccounts = Profile.LinkedAccounts,
                    Address = Profile.Address,
                    Nationality = Profile.Nationality,
                    Languages=currentUserLanguages,
                    Skills=currentUserSkill,
                    Experience=currentUserExperience,
                    VisaStatus= Profile.VisaStatus,
                    VisaExpiryDate=Profile.VisaExpiryDate,
                    ProfilePhoto=Profile.ProfilePhoto,
                    ProfilePhotoUrl=Profile.ProfilePhotoUrl,
                    JobSeekingStatus=Profile.JobSeekingStatus
                };
                return result;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public async Task<bool> UpdateTalentProfile(TalentProfileViewModel model, string updaterId)
        {
            try
            {
                User user = await _userRepository.GetByIdAsync(updaterId);
                user.LinkedAccounts = model.LinkedAccounts;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Description = model.Description;
                user.Summary = model.Summary;
                user.Address = model.Address;
                user.Nationality = model.Nationality;
                user.VisaStatus = model.VisaStatus;
                user.VisaExpiryDate = model.VisaExpiryDate;
                user.JobSeekingStatus = model.JobSeekingStatus;
                //user.ProfilePhoto = model.ProfilePhoto;
                //user.ProfilePhotoUrl = "C:/MVP_Studio/Task_3/talent-standard-tasks-master/Talent.Services.Profile/images/";
                await _userRepository.Update(user);
                return true;
            }
            catch
            { 
                return false;
            }
        }
        public async Task<EmployerProfileViewModel> GetEmployerProfile(string Id, string role)
        {

            Employer profile = null;
            switch (role)
            {
                case "employer":
                    profile = (await _employerRepository.GetByIdAsync(Id));
                    break;
                case "recruiter":
                    profile = (await _recruiterRepository.GetByIdAsync(Id));
                    break;
            }

            var videoUrl = "";

            if (profile != null)
            {
                videoUrl = string.IsNullOrWhiteSpace(profile.VideoName)
                          ? ""
                          : await _fileService.GetFileURL(profile.VideoName, FileType.UserVideo);

                var skills = profile.Skills.Select(x => ViewModelFromSkill(x)).ToList();

                var result = new EmployerProfileViewModel
                {
                    Id = profile.Id,
                    CompanyContact = profile.CompanyContact,
                    PrimaryContact = profile.PrimaryContact,
                    Skills = skills,
                    ProfilePhoto = profile.ProfilePhoto,
                    ProfilePhotoUrl = profile.ProfilePhotoUrl,
                    VideoName = profile.VideoName,
                    VideoUrl = videoUrl,
                    DisplayProfile = profile.DisplayProfile,
                };
                return result;
            }

            return null;
        }
        public async Task<bool> UpdateEmployerProfile(EmployerProfileViewModel employer, string updaterId, string role)
        {
            try
            {
                if (employer.Id != null)
                {
                    switch (role)
                    {
                        case "employer":
                            Employer existingEmployer = (await _employerRepository.GetByIdAsync(employer.Id));
                            existingEmployer.CompanyContact = employer.CompanyContact;
                            existingEmployer.PrimaryContact = employer.PrimaryContact;
                            existingEmployer.ProfilePhoto = employer.ProfilePhoto;
                            existingEmployer.ProfilePhotoUrl = employer.ProfilePhotoUrl;
                            existingEmployer.DisplayProfile = employer.DisplayProfile;
                            existingEmployer.UpdatedBy = updaterId;
                            existingEmployer.UpdatedOn = DateTime.Now;

                            var newSkills = new List<UserSkill>();
                            foreach (var item in employer.Skills)
                            {
                                var skill = existingEmployer.Skills.SingleOrDefault(x => x.Id == item.Id);
                                if (skill == null)
                                {
                                    skill = new UserSkill
                                    {
                                        Id = ObjectId.GenerateNewId().ToString(),
                                        IsDeleted = false
                                    };
                                }
                                UpdateSkillFromView(item, skill);
                                newSkills.Add(skill);
                            }
                            existingEmployer.Skills = newSkills;

                            await _employerRepository.Update(existingEmployer);
                            break;

                        case "recruiter":
                            Recruiter existingRecruiter = (await _recruiterRepository.GetByIdAsync(employer.Id));
                            existingRecruiter.CompanyContact = employer.CompanyContact;
                            existingRecruiter.PrimaryContact = employer.PrimaryContact;
                            existingRecruiter.ProfilePhoto = employer.ProfilePhoto;
                            existingRecruiter.ProfilePhotoUrl = employer.ProfilePhotoUrl;
                            existingRecruiter.DisplayProfile = employer.DisplayProfile;
                            existingRecruiter.UpdatedBy = updaterId;
                            existingRecruiter.UpdatedOn = DateTime.Now;

                            var newRSkills = new List<UserSkill>();
                            foreach (var item in employer.Skills)
                            {
                                var skill = existingRecruiter.Skills.SingleOrDefault(x => x.Id == item.Id);
                                if (skill == null)
                                {
                                    skill = new UserSkill
                                    {
                                        Id = ObjectId.GenerateNewId().ToString(),
                                        IsDeleted = false
                                    };
                                }
                                UpdateSkillFromView(item, skill);
                                newRSkills.Add(skill);
                            }
                            existingRecruiter.Skills = newRSkills;
                            await _recruiterRepository.Update(existingRecruiter);

                            break;
                    }
                    return true;
                }
                return false;
            }
            catch (MongoException e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployerPhoto(string employerId, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            List<string> acceptedExtensions = new List<string> { ".jpg", ".png", ".gif", ".jpeg" };

            if (fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))
            {
                return false;
            }

            var profile = (await _employerRepository.Get(x => x.Id == employerId)).SingleOrDefault();

            if (profile == null)
            {
                return false;
            }

            var newFileName = await _fileService.SaveFile(file, FileType.ProfilePhoto);

            if (!string.IsNullOrWhiteSpace(newFileName))
            {
                var oldFileName = profile.ProfilePhoto;

                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    await _fileService.DeleteFile(oldFileName, FileType.ProfilePhoto);
                }

                profile.ProfilePhoto = newFileName;
                profile.ProfilePhotoUrl = await _fileService.GetFileURL(newFileName, FileType.ProfilePhoto);

                await _employerRepository.Update(profile);
                return true;
            }

            return false;

        }

        public async Task<bool> AddEmployerVideo(string employerId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTalentPhoto(string talentId, IFormFile imageFile)
        {
            var fileExtension = Path.GetExtension(imageFile.FileName);
            List<string> acceptedExtensions = new List<string> { ".jpg", ".png", ".gif", ".jpeg" };

            if (fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))
            {
                return false;
            }

            var profile = (await _userRepository.Get(x => x.Id == talentId)).SingleOrDefault();

            if (profile == null)
            {
                return false;
            }
            var newFileName = await _fileService.SaveFile(imageFile, FileType.ProfilePhoto);

            if (!string.IsNullOrWhiteSpace(newFileName))
            {
                var oldFileName = profile.ProfilePhoto;

                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    await _fileService.DeleteFile(oldFileName, FileType.ProfilePhoto);
                }

                profile.ProfilePhoto = newFileName;
                profile.ProfilePhotoUrl = await _fileService.GetFileURL(newFileName, FileType.ProfilePhoto);

                await _userRepository.Update(profile);
                return true;
            }

            return false;


        }

        public async Task<bool> AddTalentVideo(string talentId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();

        }

        public async Task<bool> RemoveTalentVideo(string talentId, string videoName)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTalentCV(string talentId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetTalentSuggestionIds(string employerOrJobId, bool forJob, int position, int increment)
        {
            //Your code here;
            throw new NotImplementedException();
        }
        #region Build Views from Model
        protected TalentSnapshotViewModel BuildTalentSnapshot(User user)
        {
            var videoUrl = _fileService.GetFileURL(user.VideoName, FileType.UserVideo);
            var photoUrl = _fileService.GetFileURL(user.ProfilePhoto, FileType.ProfilePhoto);
            string name = string.Format("{0} {1}", user.FirstName, user.LastName);
            UserExperience latest = user.Experience.OrderByDescending(x => x.End).FirstOrDefault();
            String level, employment;
            List<string> skills = user.Skills.Select(x => x.Skill).ToList();
           
            if (latest != null)
            {
                level = latest.Position;
                employment = latest.Company;
            }
            else
            {
                level = "Unknown";
                employment = "Unknown";
            }
            return new TalentSnapshotViewModel
            {
                Id = user.Id,
                Name=name,
                Level=level,
                CurrentEmployment= employment,
                Skills=skills,
                CVUrl=user.CvName,
                PhotoId=user.ProfilePhotoUrl,
                LinkedAccounts=user.LinkedAccounts
            };
        }
        #endregion
        public async Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(string employerOrJobId, bool forJob, int position, int increment)
        {
            var profile = await _employerRepository.GetByIdAsync(employerOrJobId);
            var talentProfiles = _userRepository.GetQueryable().Skip(position).Take(increment);
            if (profile != null)
            {
                var newTalentProfiles = new List<TalentSnapshotViewModel>();
                foreach (var item in talentProfiles)
                {
                    newTalentProfiles.Add(BuildTalentSnapshot(item));
                }
                return newTalentProfiles;
            }
            return null;

        }

        public async Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(IEnumerable<string> ids)
        {
            try
            {
                User talent = null;
                talent = await _userRepository.GetByIdAsync(ids);

                var skill = await _userSkillRepository.Get(x => x.UserId == talent.Id && x.IsDeleted == false);
                var currentUserSkill = skill.Select(x => ViewModelFromSkill(x)).ToList();

                var experience = await _userExperienceRepository.Get(x => x.UserId == talent.Id && x.IsDeleted == false);
                var currentUserExperience = experience.Select(x => ViewModelFromExperience(x)).ToList();

                talent.ProfilePhotoUrl = string.IsNullOrWhiteSpace(talent.ProfilePhoto)
                          ? ""
                          : await _fileService.GetFileURL(talent.ProfilePhoto, FileType.ProfilePhoto);

                var result = new TalentSnapshotViewModel
                {
                    Id = talent.Id,
                    Name = talent.FirstName,
                    PhotoId = talent.ProfilePhotoUrl,
                    CVUrl = talent.CvName,
                    Summary = talent.Summary,
                    // CurrentEmployment= currentUserExperience,
                    Visa = talent.VisaStatus,
                    // Level=talent.JobSeekingStatus,
                    //Skills=talent.Skills,
                };
                return (IEnumerable<TalentSnapshotViewModel>)result;
            }
            catch
            {
                return null;
            }
            
        }

        #region TalentMatching

        public async Task<IEnumerable<TalentSuggestionViewModel>> GetFullTalentList()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public IEnumerable<TalentMatchingEmployerViewModel> GetEmployerList()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentMatchingEmployerViewModel>> GetEmployerListByFilterAsync(SearchCompanyModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSuggestionViewModel>> GetTalentListByFilterAsync(SearchTalentModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSuggestion>> GetSuggestionList(string employerOrJobId, bool forJob, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> AddTalentSuggestions(AddTalentSuggestionList selectedTalents)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #endregion
        #region Update from View
        protected void UpdateSkillFromView(AddSkillViewModel model, UserSkill original)
        {
            original.ExperienceLevel = model.Level;
            original.Skill = model.Name;
        }
        #endregion
        #region ManageClients

        public async Task<IEnumerable<ClientViewModel>> GetClientListAsync(string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<ClientViewModel> ConvertToClientsViewAsync(Client client, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalTalentsForClient(string clientId, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();

        }

        public async Task<Employer> GetEmployer(string employerId)
        {
            return await _employerRepository.GetByIdAsync(employerId);
        }
        #endregion

    }
}
