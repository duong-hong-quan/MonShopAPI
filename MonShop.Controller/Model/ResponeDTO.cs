﻿namespace MonShop.Controller.Model
{
    public class ResponeDTO
    {
        public object? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

    }
}
