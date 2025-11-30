# 🎓 UAMPass

> **Conectando el talento universitario con oportunidades reales.**

![.NET Core](https://img.shields.io/badge/.NET%20Core-7.0-purple?style=for-the-badge&logo=.net)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-blueviolet?style=for-the-badge&logo=bootstrap)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Elephant-336791?style=for-the-badge&logo=postgresql)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen?style=for-the-badge)

## 📄 Descripción

**UAMPass** es una plataforma web integral desarrollada bajo el patrón de arquitectura **ASP.NET Core MVC**. Su misión es servir como un puente académico-profesional, permitiendo la gestión eficiente de estudiantes y empresas.

El objetivo principal es facilitar que los estudiantes de la **Universidad Americana (UAM)** encuentren oportunidades de pasantías y que las empresas puedan captar talento joven de manera directa.

---

## 🛠️ Tecnologías Utilizadas

Este proyecto ha sido construido utilizando estándares modernos de desarrollo web:

* **Backend:** ASP.NET Core MVC 7.0 (C#)
* **ORM:** Entity Framework Core (Manejo de datos)
* **Base de Datos:** PostgreSQL (Neon Tech)
* **Frontend:** Razor Views (.cshtml) + HTML5
* **Estilos:** Bootstrap 5 (Diseño Responsivo y Componentes UI)

---

## 📂 Estructura del Proyecto

La arquitectura sigue el patrón **Modelo-Vista-Controlador (MVC)** para asegurar un código limpio y escalable:

```text
UAMPass/
├── 📂 Controllers/       # El "Cerebro" de la aplicación
│   ├── EstudiantesController.cs
│   └── AuthController.cs
│   └── (Lógica de negocio y manejo de peticiones HTTP)
│
├── 📂 Models/            # La "Estructura" de datos
│   ├── Dto/              # Data Transfer Objects (LoginDTO, etc.)
│   └── (Definiciones de tablas y ViewModels)
│
├── 📂 Views/             # La "Cara" de la aplicación
│   ├── Login.cshtml      # Combinación de C# (Razor) y HTML
│   ├── Dashboard.cshtml
│   └── (Interfaz gráfica de usuario)
│
└── 📂 wwwroot/           # Archivos Estáticos
    ├── css/              # Hojas de estilo personalizadas
    ├── js/               # Scripts interactivos
    └── images/           # Recursos gráficos
