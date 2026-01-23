// p.413 [Add] 依存性を伴うフィルター ーーIFilterFactoryインターフェイス
using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNetCore.Filters;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false)]
public class LogExceptionAttribute : Attribute, 
                                     IFilterFactory // 継承。※IFilterFactoryもIFilterMetadataを継承しているため、一般的なフィルターと同じ要領で使用できる。
{
    // フィルターが再利用可能か。
    //  ※CreateInstance(IServiceProvider)の結果がリクエスト間で再利用可能かどうかを示す値。
    //  ※たとえば「スコープ付き(Scoped)」「一時的(Transient)」として登録されているフィルターは再利用可能(true)とはしない。
    public bool IsReusable => false; // 固定でfalseとしておく
    //-------------------------------------------------------------------------------------------------
    // ■ AIによる概要
    //  bool IsReusable => false は C# の構文で、プロパティ IsReusable が常に false を返すことを示します。
    //
    //  これは、特にASP.NETのHTTPハンドラー(IHttpHandlerインターフェース)の実装でよく見られるパターンです。
    //   ・true の場合: 
    //      ハンドラーのインスタンスが処理要求間でキャッシュされ、再利用される可能性があります。
    //      これによりパフォーマンスが向上します。
    //   ・false の場合: 
    //      各HTTP要求に対して新しいハンドラーのインスタンスが作成されます。
    //      これにより、インスタンスごとに状態を安全に管理できますが、オーバーヘッドは大きくなります。
    //-------------------------------------------------------------------------------------------------


    // フィルター生成の本体
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        // フィルターそのものはIServiceProvider＃GetRequiredServiceメソッドで、登録済みのサービスとして取得する
        // ※Program.csでサービスとしてアプリに登録されている前提
        LogExceptionFilter filter = serviceProvider.GetRequiredService<LogExceptionFilter>();

        /* (必要であれば)ここで、取得したフィルターオブジェクトに対してプロパティなどを設定してから戻り値として返す */

        return filter;
    }
}
