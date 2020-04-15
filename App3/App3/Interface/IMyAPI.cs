using App3.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App3.Interface
{
    public interface IMyAPI
    {
        [Get("/Form101Indicators/likeIndCode/{indcode}/take_cnt_rows/{cnt_rows}")]
        Task<List<IndCodekInfo>> Get_indcode(string indcode, int cnt_rows);



        [Get("/Banks/allbanks/{shortname}/take_cnt_rows/{cnt_rows}")]
        Task<List<BankInfo>> Get_allbanks(string shortname, int cnt_rows);









        [Get("/F101/actives/{dt}")]
        Task<List<F101_actives>> GetF101_actives(string dt);



        [Get("/F101/passives/{dt}")]
        Task<List<F101_passives>> GetF101_passives(string dt);



        [Get("/F101/actives_top_n/{dt_slice}/{dt_from}/{top_n}")]
        Task<List<F101_actives_top_n>> GetF101_actives_top_n(string dt_slice, string dt_from, int top_n);



        [Get("/F101/passives_top_n/{dt_slice}/{dt_from}/{top_n}")]
        Task<List<F101_passives_top_n>> GetF101_passives_top_n(string dt_slice, string dt_from, int top_n);






        [Get("/F101/bank_actives/{dt}/{bank}")]
        Task<List<F101_actives_top_n>> GetF101_bank_actives(string dt, string bank);

        [Get("/F101/bank_passives/{dt}/{bank}")]
        Task<List<F101_passives_top_n>> GetF101_bank_passives(string dt, string bank);









        /*101f*/
        [Get("/F101/data/{MODE}/{tip}/{str}/{dt_from}/{dt_to}")]
        Task<List<t_application_F101_allbanks>> GetF101_data(string MODE, int tip, string str, string dt_from, string dt_to);

        [Get("/F101/data/{MODE}/{tip}/{str}/{dt_from}/{dt_to}")]
        Task<List<t_application_F101_allbanks>> GetF101_groups(string MODE, int tip, string str, string dt_from, string dt_to);


        /*Regnumber info*/
        /*
                       @MODE = 'search_bar_withRF'
                       @MODE = 'bankinfo'
                       @MODE = 'search_bar_withoutRF'


                       if (MODE == "bankinfo")
                               {
                                   SqlParameter parameter1 = new SqlParameter("@MODE", MODE);
                                   SqlParameter parameter2 = new SqlParameter("@regnumber", n);
                                   return entities.Database.SqlQuery<t_BIC_CO>(" exec p_application_bankinfo @MODE,  null, @regnumber", parameter1, parameter2).ToList();
                               }

                               else

                               {
                                   SqlParameter parameter1 = new SqlParameter("@MODE", MODE);
                                   SqlParameter parameter2 = new SqlParameter("@shortname", shortname);
                                   return entities.Database.SqlQuery<t_BIC_CO>(" exec p_application_bankinfo @MODE,  @shortname, null", parameter1, parameter2).ToList().Take(n);
                               }



        */

        [Get("/Banks/bankinfo/{MODE}/{shortname}/{n}")]
        Task<List<BankInfo>> Getbankinfo(string MODE, string shortname, string n);




        [Get("/Banks/dates/{obj}")]
        Task<List<t_dates>> t_dates(string obj);


        /*102f*/
        [Get("/F102/data/{slice}/{field_id}/{regn}/{dt_from}/{dt_to}")]
        Task<List<Dataset_F102>> GetF102_data(string slice, int field_id, string regn, string dt_from, string dt_to);
        


    }
}
