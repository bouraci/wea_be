﻿namespace WEA_BE.DTO;

public class CommentRequestDto
{
    public string content { get; set; }
    public int bookId { get; set; }
    public string userName { get; set; }
}