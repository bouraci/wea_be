﻿
using WEA_BE.DTO;

namespace WEA_BE.Services
{
    /// <summary>
    /// Služba pro správu autentizace uživatelů, zahrnující registraci a přihlášení.
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// Zaregistruje nového uživatele s poskytnutými údaji.
        /// </summary>
        /// <param name="name">Jméno uživatele.</param>
        /// <param name="username">Uživatelské jméno.</param>
        /// <param name="password">Heslo uživatele.</param>
        /// <returns>Vrací true, pokud registrace proběhla úspěšně, nebo false, pokud uživatel s daným uživatelským jménem již existuje.</returns>
        Task<UserDto?> LoginAsync(string username, string password);
        /// <summary>
        /// Přihlašuje uživatele na základě uživatelského jména a hesla.
        /// </summary>
        /// <param name="username">Uživatelské jméno.</param>
        /// <param name="password">Heslo uživatele.</param>
        /// <returns>Vrací DTO uživatele, pokud je přihlášení úspěšné, nebo null, pokud je přihlášení neúspěšné.</returns>
        Task<bool> RegisterAsync(string name, string username, string password);
    }
}