using EarningsCalculator;
using DebtPaymentCalculator;
using DSCRCalculator;


namespace AcquisitionInc;
public class financialSummary
{
    public static void Main(string[] args)
    {

    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddCors(options => {
        options.AddPolicy("VueAppPolicy", policy => {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddMemoryCache();

    // builder.Services.AddScoped<EarningService>();
    builder.Services.AddScoped<DebtService>();
    builder.Services.AddScoped<DSCRService>();
    
    var app = builder.Build();

    app.UseCors("VueAppPolicy");

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();


    //    EarningCalculator compiler = new();
    //    DebtController compiler2 = new(new DebtService());
    //    compiler.RunProgram();
    //    compiler2.RunProgram();

    }
    
}
