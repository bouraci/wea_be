﻿using AutoMapper;
using EFModels.Data;
using WEA_BE.DTO;

namespace WEA_BE.Services;

/// <summary>
/// Služba pro správu knih, zahrnující získávání knih na základě různých kritérií.
/// </summary>
public class BookService : IBookService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;

    /// <summary>
    /// Konstruktor pro inicializaci služby s kontextem databáze a mapováním objektů.
    /// </summary>
    /// <param name="ctx">Kontext databáze pro přístup k datům knih.</param>
    /// <param name="mapper">Automapper pro mapování mezi entitami a DTO.</param>
    public BookService(DatabaseContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    /// Vrací seznam knih na základě zadaných filtrů a podporuje stránkování.
    /// </summary>
    /// <param name="title">Název knihy (nepovinné).</param>
    /// <param name="author">Autor knihy (nepovinné).</param>
    /// <param name="genre">Žánr knihy (nepovinné).</param>
    /// <param name="publicationYear">Rok vydání knihy (nepovinné).</param>
    /// <param name="minRating">Minimální hodnocení knihy (nepovinné).</param>
    /// <param="maxRating">Maximální hodnocení knihy (nepovinné).</param>
    /// <param name="page">Číslo stránky (výchozí hodnota 1).</param>
    /// <param name="pageSize">Počet položek na stránku (maximálně 100).</param>
    /// <returns>Seznam knih včetně celkového počtu záznamů.</returns>
    public (List<BookDto>, int totalRecords) GetBooks(string? title, string? author, string? genre, int? publicationYear, double? minRating, double? maxRating, int page, int pageSize)
    {
        if (pageSize > 100) pageSize = 100;

        var query = _ctx.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.ToLower().Contains(title.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.Authors.ToLower().Contains(author.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(b => b.Genre.ToLower().Contains(genre.Trim().ToLower()));

        if (publicationYear is not null)
            query = query.Where(b => b.PublicationYear == publicationYear);

        if (minRating is not null)
            query = query.Where(b => b.Rating >= minRating);

        if (maxRating is not null)
            query = query.Where(b => b.Rating <= maxRating);

        var totalRecords = query.Count();

        var books = query.Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();

        var bookDtos = _mapper.Map<List<BookDto>>(books);
        return (bookDtos, totalRecords);
    }

    /// <summary>
    /// Vrací konkrétní knihu na základě jejího ID.
    /// </summary>
    /// <param name="id">ID knihy.</param>
    /// <returns>DTO objekt knihy.</returns>
    public BookDto GetBookById(int id)
    {
        var book = _ctx.Books.SingleOrDefault(x => x.Id == id);
        return _mapper.Map<BookDto>(book);
    }
}
