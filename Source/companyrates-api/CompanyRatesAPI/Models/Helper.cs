using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace CompanyRatesAPI.Models
{
    public class Helper
    {
        internal static bool isAuthenticated(string email, string passwordHash)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                email = email.ToLower();
                string hash = GetStringSha256Hash(passwordHash);
                return db.Users.Count(x => x.Email == email && x.PasswordHash == hash) > 0;
            }
        }

        internal static void sendEmail(Email email)
        {
            var fromAddress = new MailAddress("companyrates2018@gmail.com", "Company Rates");
            var toAddress = new MailAddress("tadej.rola@gmail.com", "Tadej Rola");
            const string fromPassword = "table-droppers2018";
            string subject = "[COMPANY RATES CONTACT FORM] - " + email.Reason + " [FROM: " + email.From + "]";
            string body = email.Text;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        internal static void removeCompanyAll()
        {
            using (var db = new CompanyRatesAPIContext())
            {
                List<Company> companies = db.Companies.ToList();
                if (companies.Count > 0)
                {
                    foreach (var company in companies)
                    {
                        var votesCompanies = db.VoteCompanies.Where(x => x.Company_FK == company.CompanyID).ToList();
                        db.VoteCompanies.RemoveRange(votesCompanies);
                        var reviews = db.Reviews.Where(x => x.Company_FK == company.CompanyID).ToList();
                        foreach (var a in reviews)
                        {
                            var votes = db.VoteReviews.Where(x => x.Review_FK == a.ReviewID).ToList();
                            db.VoteReviews.RemoveRange(votes);
                        }
                        db.Reviews.RemoveRange(reviews);
                        var user = db.Users.FirstOrDefault(x => x.UserID == company.User_FK);
                        var sessions = db.LoginSessions.Where(x => x.User_FK == user.UserID).ToList();
                        db.LoginSessions.RemoveRange(sessions);
                        db.Companies.Remove(company);
                        db.Users.Remove(user);
                        db.SaveChanges();
                    }
                }
            }
        }

        internal static List<Review> getReviewByCompany(int id)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var review = db.Reviews.Where(x => x.Company_FK == id).Include(x => x.UserFK).ToList();
                foreach (var r in review)
                {
                    r.UserFK.PasswordHash = "";
                }
                return review;
            }
        }

        internal static bool removeCompany(int id)
        {
            //REMOVE VOTECOMPANIES
            //REMOVE VOTEREVIEWS
            //REMOVE REVIEWS
            //REMOVE COMPANY
            //REMOVE SESSIONS
            //REMOVE USER

            using (var db = new CompanyRatesAPIContext())
            {
                Company company = db.Companies.FirstOrDefault(x => x.CompanyID == id);
                if (company == null)
                {
                    return false;
                }
                else
                {
                    var votesCompanies = db.VoteCompanies.Where(x => x.Company_FK == id).ToList();
                    db.VoteCompanies.RemoveRange(votesCompanies);
                    var reviews = db.Reviews.Where(x => x.Company_FK == id).ToList();
                    foreach (var a in reviews)
                    {
                        var votes = db.VoteReviews.Where(x => x.Review_FK == a.ReviewID).ToList();
                        db.VoteReviews.RemoveRange(votes);
                    }
                    db.Reviews.RemoveRange(reviews);
                    var user = db.Users.FirstOrDefault(x => x.UserID == company.User_FK);
                    var sessions = db.LoginSessions.Where(x => x.User_FK == user.UserID).ToList();
                    db.LoginSessions.RemoveRange(sessions);
                    db.Companies.Remove(company);
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return true;
                }
            }
        }

        internal static bool removeReview(int id)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                Review review = db.Reviews.FirstOrDefault(x => x.ReviewID == id);
                if (review == null)
                {
                    return false;
                }
                else
                {
                    var votes = db.VoteReviews.Where(x => x.Review_FK == id).ToList();
                    db.VoteReviews.RemoveRange(votes);
                    db.SaveChanges();

                    db.Reviews.Remove(review);
                    db.SaveChanges();
                    return true;
                }
            }
        }

        internal static List<Review> getReviews()
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var list = db.Reviews.ToList();
                return list;
            }
        }

        internal static List<User> GetUsers()
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var list = db.Users.ToList();
                foreach (var a in list)
                {
                    a.PasswordHash = "";
                }
                return list;
            }
        }

        internal static User GetUser(int id)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                user.PasswordHash = "";
                return user;
            }
        }

        internal static List<Company> getCompanies()
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.Companies.ToList();
            }
        }

        internal static void updateReview(int id, Review review)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var _review = db.Reviews.Where(x => x.ReviewID == id).FirstOrDefault();
                if (_review != null)
                {
                    //TODO
                }
            }
        }

        internal static Company updateCompany(int id, Company company)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var _company = db.Companies.Where(x => x.CompanyID == id).FirstOrDefault();
                if (_company != null)
                {
                    _company.Address = company.Address;
                    _company.City = company.City;
                    _company.Country = company.Country;
                    _company.LogoUrl = company.LogoUrl;
                    _company.Name = company.Name;
                    _company.Website = company.Website;
                    _company.Verified = company.Verified;
                    db.SaveChanges();
                    if (_company.UserFK != null)
                    {
                        _company.UserFK.PasswordHash = "";
                    }
                    return _company;
                }
                return null;
            }
        }

        internal static bool VoteReview(VoteReview voteReview)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                if (db.VoteReviews.Where(x => x.User_FK == voteReview.User_FK && x.Review_FK == voteReview.Review_FK).ToList().Count == 0)
                {
                    if (voteReview.Value > 1 || voteReview.Value == 1)
                        voteReview.Value = 1;
                    else
                        voteReview.Value = -1;
                    voteReview.ReviewFK = db.Reviews.Where(x => x.ReviewID == voteReview.Review_FK).FirstOrDefault();
                    voteReview.UserFK = db.Users.Where(x => x.UserID == voteReview.User_FK).FirstOrDefault();
                    db.VoteReviews.Add(voteReview);
                    db.SaveChanges();
                    var review = db.Reviews.Where(x => x.ReviewID == voteReview.Review_FK).FirstOrDefault();
                    review.TotalRating += voteReview.Value;
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        internal static bool VoteCompany(VoteCompany voteCompany)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                if (db.VoteCompanies.Where(x => x.User_FK == voteCompany.User_FK && x.Company_FK == voteCompany.Company_FK).ToList().Count == 0)
                {
                    if (voteCompany.Value > 1 || voteCompany.Value == 1)
                        voteCompany.Value = 1;
                    else
                        voteCompany.Value = -1;
                    voteCompany.CompanyFK = db.Companies.Where(x => x.CompanyID == voteCompany.Company_FK).FirstOrDefault();
                    voteCompany.UserFK = db.Users.Where(x => x.UserID == voteCompany.User_FK).FirstOrDefault();
                    db.VoteCompanies.Add(voteCompany);
                    db.SaveChanges();
                    var company = db.Companies.Where(x => x.CompanyID == voteCompany.Company_FK).FirstOrDefault();
                    company.TotalRating += voteCompany.Value;
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        internal static LoginSession getSession(string sessionkey)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.LoginSessions.Where(x => x.SessionKey == sessionkey).FirstOrDefault();
            }
        }

        internal static Company getCompany(int id)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.Companies.Where(x => x.CompanyID == id).FirstOrDefault();
            }
        }

        internal static Review createReview(Review review)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                review.CompanyFK = db.Companies.Where(x => x.CompanyID == review.Company_FK).FirstOrDefault();
                review.UserFK = db.Users.Where(x => x.UserID == review.User_FK).FirstOrDefault();
                db.Reviews.Add(review);
                db.SaveChanges();
                review.UserFK.PasswordHash = "";
                return review;
            }
        }

        internal static User UpdateUser(int id, User user)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var _user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                if (_user != null)
                {
                    _user.PasswordHash = GetStringSha256Hash(user.PasswordHash);
                    _user.isAdmin = user.isAdmin;
                    db.SaveChanges();
                    _user.PasswordHash = "";
                    return _user;
                }
                return null;
            }
        }

        internal static LoginSession createSession(string email)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenData = new byte[32];
                    rng.GetBytes(tokenData);
                    var user = db.Users.Where(x => x.Email == email).FirstOrDefault();
                    if (user != null)
                    {
                        string token = Convert.ToBase64String(tokenData);
                        token = token.Trim(' ');
                        token = token.Replace('+', 'a');
                        LoginSession loginSession = new LoginSession();
                        loginSession.User_FK = user.UserID;
                        loginSession.UserFK = user;
                        loginSession.SessionKey = token;
                        loginSession.ValidFrom = DateTime.Now;
                        loginSession.ValidTo = DateTime.Now.AddDays(1);
                        db.LoginSessions.Add(loginSession);
                        db.SaveChanges();
                        loginSession.UserFK.PasswordHash = "";
                        return loginSession;
                    }
                    return null;
                }
            }
        }

        internal static User createUser(User user)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                user.Email = user.Email.ToLower();
                user.isAdmin = false;
                string hash = GetStringSha256Hash(user.PasswordHash);
                user.PasswordHash = hash;
                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
        }

        internal static bool EmailExists(string email)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.Users.Count(e => e.Email == email) > 0;
            }
        }

        internal static Company createCompany(Company company)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                company.UserFK = db.Users.Where(x => x.UserID == company.User_FK).FirstOrDefault();
                company.Verified = false;
                company.TotalRating = 0;
                db.Companies.Add(company);
                db.SaveChanges();
                return company;
            }
        }

        internal static void updateCompany(Company company)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var _company = db.Companies.Where(x => x.CompanyID == company.CompanyID).FirstOrDefault();
                _company = company;
                _company.UserFK = db.Users.Where(x => x.UserID == company.UserFK.UserID).FirstOrDefault();
                db.SaveChanges();
            }
        }

        internal static bool CompanyExists(int id)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.Companies.Count(e => e.CompanyID == id) > 0;
            }
        }

        internal static bool isUser(string sessionkey)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var session = db.LoginSessions.Include(x => x.UserFK).Where(x => x.SessionKey == sessionkey).FirstOrDefault();
                if (session != null)
                {
                    if (session.UserFK.isCompany == false)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        internal static bool isCompany(string sessionkey)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var session = db.LoginSessions.Include(x => x.UserFK).Where(x => x.SessionKey == sessionkey).FirstOrDefault();
                if (session != null)
                {
                    if (session.UserFK.isCompany == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        internal static bool isAdmin(string sessionkey)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var session = db.LoginSessions.Include(x => x.UserFK).Where(x => x.SessionKey == sessionkey).FirstOrDefault();
                if (session != null)
                {
                    if (session.UserFK.isAdmin == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        internal static User getUser(int userID)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            }
        }

        internal static bool removeSession(string sessionkey)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var session = db.LoginSessions.Where(x => x.SessionKey == sessionkey).FirstOrDefault();
                if (session != null)
                {
                    db.LoginSessions.Remove(session);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        internal static bool isSessionValid(string sessionkey)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                var session = db.LoginSessions.Where(x => x.SessionKey == sessionkey).FirstOrDefault();
                if (session != null)
                {
                    if (session.ValidTo > DateTime.Now)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}