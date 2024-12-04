using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Models;

public partial class MotoWebsiteContext : DbContext
{
    public MotoWebsiteContext()
    {
    }

    public MotoWebsiteContext(DbContextOptions<MotoWebsiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<LibraryImage> LibraryImages { get; set; }

    public virtual DbSet<MotoBike> MotoBikes { get; set; }

    public virtual DbSet<MotoLibrary> MotoLibraries { get; set; }

    public virtual DbSet<MotoType> MotoTypes { get; set; }

    public virtual DbSet<MotoVersion> MotoVersions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VersionColor> VersionColors { get; set; }

    public virtual DbSet<VersionImage> VersionImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DatDeptrai;Initial Catalog=MotoWebsite;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.MaHangSanXuat).HasName("PK__Brand__977119FCFEE33B56");

            entity.ToTable("Brand");

            entity.Property(e => e.MaHangSanXuat).HasMaxLength(50);
            entity.Property(e => e.QuocGia).HasMaxLength(100);
            entity.Property(e => e.TenHangSanXuat).HasMaxLength(100);
        });

        modelBuilder.Entity<LibraryImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__LibraryI__7516F70C7A36A911");

            entity.ToTable("LibraryImage");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MaLibrary).HasMaxLength(50);

            entity.HasOne(d => d.MaLibraryNavigation).WithMany(p => p.LibraryImages)
                .HasForeignKey(d => d.MaLibrary)
                .HasConstraintName("FK__LibraryIm__MaLib__3D5E1FD2");
        });

        modelBuilder.Entity<MotoBike>(entity =>
        {
            entity.HasKey(e => e.MaXe).HasName("PK__MotoBike__272520CD572A7FCC");

            entity.ToTable("MotoBike");

            entity.Property(e => e.MaXe).HasMaxLength(50);
            entity.Property(e => e.AnhMoTaUrl).HasMaxLength(255);
            entity.Property(e => e.CongSuatToiDa).HasMaxLength(255);
            entity.Property(e => e.DoCaoGamXe).HasMaxLength(255);
            entity.Property(e => e.DoCaoYen).HasMaxLength(255);
            entity.Property(e => e.DungTichBinhXang).HasMaxLength(255);
            entity.Property(e => e.DungTichXyLanh).HasMaxLength(255);
            entity.Property(e => e.DuongKinhHanhTrinhPittong).HasMaxLength(255);
            entity.Property(e => e.GiaBanMoTa).HasMaxLength(50);
            entity.Property(e => e.HeThongKhoiDong).HasMaxLength(255);
            entity.Property(e => e.KhoangCachTrucBanhXe).HasMaxLength(255);
            entity.Property(e => e.KichCoLop).HasMaxLength(255);
            entity.Property(e => e.KichThuoc).HasMaxLength(255);
            entity.Property(e => e.LoaiDongCo).HasMaxLength(255);
            entity.Property(e => e.MaHangSanXuat).HasMaxLength(50);
            entity.Property(e => e.MaLibrary).HasMaxLength(50);
            entity.Property(e => e.MaLoai).HasMaxLength(50);
            entity.Property(e => e.MomentCucDai).HasMaxLength(255);
            entity.Property(e => e.MucTieuThuNhienLieu).HasMaxLength(255);
            entity.Property(e => e.PhuocSau).HasMaxLength(255);
            entity.Property(e => e.PhuocTruoc).HasMaxLength(255);
            entity.Property(e => e.TenXe).HasMaxLength(100);
            entity.Property(e => e.TrongLuong).HasMaxLength(255);
            entity.Property(e => e.TySoNen).HasMaxLength(255);

            entity.HasOne(d => d.MaHangSanXuatNavigation).WithMany(p => p.MotoBikes)
                .HasForeignKey(d => d.MaHangSanXuat)
                .HasConstraintName("FK__MotoBike__MaHang__4222D4EF");

            entity.HasOne(d => d.MaLibraryNavigation).WithMany(p => p.MotoBikes)
                .HasForeignKey(d => d.MaLibrary)
                .HasConstraintName("FK__MotoBike__MaLibr__403A8C7D");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.MotoBikes)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__MotoBike__MaLoai__412EB0B6");
        });

        modelBuilder.Entity<MotoLibrary>(entity =>
        {
            entity.HasKey(e => e.MaLibrary).HasName("PK__MotoLibr__FEC93B496B7ACBB1");

            entity.ToTable("MotoLibrary");

            entity.Property(e => e.MaLibrary).HasMaxLength(50);
        });

        modelBuilder.Entity<MotoType>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__MotoType__730A5759DF861109");

            entity.ToTable("MotoType");

            entity.Property(e => e.MaLoai).HasMaxLength(50);
            entity.Property(e => e.DoiTuongSuDung).HasMaxLength(100);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<MotoVersion>(entity =>
        {
            entity.HasKey(e => e.MaVersion).HasName("PK__MotoVers__9F72C1E11349CF5A");

            entity.ToTable("MotoVersion");

            entity.Property(e => e.MaVersion).HasMaxLength(50);
            entity.Property(e => e.GiaBanVersion).HasMaxLength(50);
            entity.Property(e => e.MaXe).HasMaxLength(50);
            entity.Property(e => e.TenVersion).HasMaxLength(100);

            entity.HasOne(d => d.MaXeNavigation).WithMany(p => p.MotoVersions)
                .HasForeignKey(d => d.MaXe)
                .HasConstraintName("FK__MotoVersio__MaXe__44FF419A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C051217AE");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E459930A66").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(25);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(25);
        });

        modelBuilder.Entity<VersionColor>(entity =>
        {
            entity.HasKey(e => e.MaVersionColor).HasName("PK__VersionC__2473925CBCB85F39");

            entity.ToTable("VersionColor");

            entity.Property(e => e.MaVersionColor).HasMaxLength(50);
            entity.Property(e => e.MaVersion).HasMaxLength(50);
            entity.Property(e => e.TenMau).HasMaxLength(50);

            entity.HasOne(d => d.MaVersionNavigation).WithMany(p => p.VersionColors)
                .HasForeignKey(d => d.MaVersion)
                .HasConstraintName("FK__VersionCo__MaVer__47DBAE45");
        });

        modelBuilder.Entity<VersionImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__VersionI__7516F70CC2BBAD58");

            entity.ToTable("VersionImage");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MaVersionColor).HasMaxLength(50);

            entity.HasOne(d => d.MaVersionColorNavigation).WithMany(p => p.VersionImages)
                .HasForeignKey(d => d.MaVersionColor)
                .HasConstraintName("FK__VersionIm__MaVer__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
