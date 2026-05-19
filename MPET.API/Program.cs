using EarningsCalculator;
using DebtPaymentCalculator;
using DSCRCalculator;
using StressTestCalculator;


namespace AcquisitionInc;
public class FinancialSummary
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
    builder.Services.AddScoped<StressTestService>();
    
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // full stack traces on 500s
}

    app.UseRouting();

    app.UseCors("VueAppPolicy");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();


    //    EarningCalculator compiler = new();
    //    DebtController compiler2 = new(new DebtService());
    //    compiler.RunProgram();
    //    compiler2.RunProgram();

    }
    
}
