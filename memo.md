# 学習メモ

## Blazor を使用した Web 開発の概要

### Blazor とは

- HTML, CSS, C#を用いたWebフレームワーク
- .NETの一部
- クロスプラットフォームで動作

#### 利点

- 再利用可能なコンポーネントを仕様してWeb UIをすばやく構築する
  - コンポーネントモデルを採用
- C#で実装できる
  - C#開発者にとって学習コストが低い
- 技術スタックが1つで済む
  - フロントエンドとバックエンドをC#一本でコード共有できる
- 差分ベースレンダリング
  - JS/TSフレームワークで採用される仮想DOMではなく、Render Treeが使用されている
  - UI要素をメモリ上で保持し、変更部分のみをDOMに反映させる仕組み
  - 仮想DOMと比べて初期ロード時間が遅くなる傾向があるらしい
- サーバーとクライアントの両方でレンダリングできる
- JavaScriptとの相互運用
  - C#コードからJSライブラリやブラウザAPIのエコシステムを使用できる

### Blazor のしくみ

- Blazorコンポーネント
  - 再利用可能なWeb UI
  - レンダリングとUIイベント処理をカプセル化
  - 事前構築済みのBlazorコンポーネントが利用可能
- UIイベント処理とデータバインディング
  - C#イベントハンドラを使用してWeb UI操作を処理する
  - コンポーネントの状態とUI要素を双方向バインディングで同期する
  - `@code` ブロックにイベント処理などを記述できる
- レンダリング
  - 既定ではサーバーで静的レンダリングしてHTMLを生成する
  - WebSocketで対話的にUI操作、更新
  - クライアントにWebAssemblyをダウンロードし、クライアントで動作
  - SPA

### Blazorの使い時

- 生産性の高いフルスタックWeb開発
- 既に.NETを使用しており、既存スキルを活用したい
- 高いパフォーマンスとスケーラビリティを備えるバックエンドが必要

適していないケース

- クライアント側の初期ダウンロードサイズと読み込み時間を完全に最適化する必要がある
  - Render Treeの初期読み込みやWASMのダウンロード時間が影響と思われる
- 別のフロントエンドフレームワークエコシステムと密接な統合を行う必要がある
  - フロントエンドはJS、バックエンドはBlazor、とはやりにくいということか
  - APIサーバーにできないこともなさそうだが、あえてBlazorでらなくてもいいFWは他にありそう
- 古いWebブラウザをサポートする必要がある

## Blazor を使って初めての Web アプリを構築する

### 開発環境の構築

- [.NET SDK](https://dotnet.microsoft.com/ja-jp/)のインストール
  - `dotnet new <template-name>` …… 新しいプロジェクトの作成
    - `dotnet new blazor`
  - `dotnet build`, `dotnet run` …… プロジェクトのビルド、実行
  - `dotnet watch` コード変更の自動適用
- Blazorツール
  - [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) 拡張機能で使用可能

### ウェブアプリを作成、実行

.NET 8.0 SDKを使ってウェブアプリを作成する。  
ターミナルで `dotnet --list-sdks` コマンドを実行し、SDKがインストールされているか確認する。

```cmd
> dotnet --list-sdks
8.0.404 [C:\Program Files\dotnet\sdk]
```

Ctrl + Shift + P キーでコマンドパレットを表示し、「.NET: 新しいプロジェクト」を選択する。`.net new project` で検索可能。

![alt text](images/02_new_project.png)

Blazor Web アプリを選択し、プロジェクト名、フォルダを作成するフォルダを選択する。

![alt text](images/02_select_blazor.png)

![alt text](images/02_solution_explorrer.png)

- `Program.cs` …… エントリーポイント。サービスとミドルウェアを構成する
- `App.razor` …… アプリのルートコンポーネント
- `Routes.razor` …… Blazorルーター
- `Components/Pages` …… アプリのWebページ
- `FirstBlazorApp.csproj` …… プロジェクトファイル
- `Properties/launchSettings.json` …… ローカル開発環境のプロファイル設定

その後、VSCodeでデバッグをしようと試みたが、うまく出来なかった。  
より具体的には、ビルドや実行は出来たが、デバッガを使ってブレークポイントで止める、ということが出来なかった。

VSCodeの適切なデバッガ選択や launch.json が不明。

仕方ないので、Visual Studioに切り替えて実施する。

新規プロジェクトで Blazor Web App を選択する。

![alt text](images/02_new_project_vs.png)

プロジェクト名を入力し、追加情報を設定して作成する。

![alt text](images/02_additional_info.png)

デバッグを開始した後、火のアイコンをアクティブにするとホットリロードを有効にできる。  
「ファイル保存時のホットリロード」を有効にしないとリロードされない？

![alt text](images/02_hot_reload.png)

### Razor コンポーネント

Razor とは

- HTML と C# に基づいたマークアップ構文
- プレーンな HTML と C# のロジックを含む
- Razor ファイルは、コンポーネントのrんだリングロジックをカプセル化する C# クラスにコンパイルされる

Razor コンポーネントとは

- レンダリングする HTML およびユーザーイベントの処理方法を再利用可能なコンポーネントとして定義する
- Razor で作成された Blazor コンポーネントは単なるC#クラスなので、任意の.NETコードを使用できる
- コンポーネント名と一致するHTMLスタイルタグを使用することで、コンポーネントを使用できる
- パラメータはpublicな `[Parameter]` 属性を持つプロパティをコンポーネントに追加することで定義し、プロパティ名に一致するHTMLスタイル属性で値を指定する
- `@page` ディレクティブでページのルートを指定する
- `@code` ブロックでC#クラスメンバーをコンポーネントに追加する
- `@rendermode InteractiveServer` ディレクトリ部を宣言すると、ブラウザからのUIイベントをサーバーが処理できるようになる
  - モードは None, Server, WebAssembly, Auto(Server and WebAssembly) が選択できる

## Blazor を使用して To Do リストを作成する

### データバインディングとイベント

データバインディングやUIイベントを処理するためには、コンポーネントが対話型である必要がある。  
`@rendermode` ディレクティブで対話型レンダリングモードを適用する。

- C# 式の値をレンダリングする場合、先頭に `@` 文字をつける
  - `( )` を使用して式の開始と終了を明示的に指定することもできる
- 制御フローを追加する場合も先頭に `@` 文字をつける

```
@if (currentCount > 3)
{
    <p>You win!</p>
}
```

```
<ul>
    @foreach (var item in items)
    {
        <li>@item.Name</li>
    }
</ul>
```

- UIイベントのコールバックは `@on` で始まり、イベント名で終わる属性を使用する
  - `@onclick` 属性でボタンクリックイベント
  - `@onchange` , `@oninput` など
  - メソッド名を指定する他、ラムダ式をインラインで定義することもできる
- UI要素の値をコード内の特定の値にバインドするには、 `@bind` 属性を使用する
  - `@` で指定する場合と比較し、ユーザー入力等のUI側での変更をC#コードで同期できる
  - 追加の修飾子を使用できる
    - `@bind:get` や `@bind:set` では値の取得、設定時のコールバックを指定できる
    - `@bind:after` 値が更新された後のコールバックを指定できる

### Todoリストを作成する

Razorコンポーネントを追加。

![alt text](images/03_add_component.png)

.NET CLIで実行する場合;

```cmd
dotnet new razorcomponent -n Todo -o Components/Pages
```

`@page` ディレクティブと `@rendermode` ディレクティブを追加する。

```
@page "/todo"
@rendermode InteractiveServer

<h3>Todo</h3>

@code {

}
```

`Layout/NavMenu.razor` を編集し、ナビゲーションメニューにページリンクを追加

```
<div class="nav-scrollable" onclick="document.querySelector('.navbar-toggler').click()">
    <nav class="flex-column">
...
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="todo">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Todo
            </NavLink>
        </div>
    </nav>
</div>
```

テキストエリアとボタンを配置し、 文字列フィールドと `@onclick` イベントのバインドを設定してTodoを追加できるようにする。

```
@page "/todo"
@rendermode InteractiveServer

<h3>Todo</h3>

<input @bind="newTodo" />
<button @onclick="AddTodo">Add todo</button>

<ul>
    @foreach (var todo in todos)
    {
        <li>@todo.Title</li>
    }
</ul>

@code {
    private List<TodoItem> todos = new();

    string newTodo = string.Empty;
    private void AddTodo()
    {
        if (!string.IsNullOrWhiteSpace(newTodo))
        {
            todos.Add(new TodoItem { Title = newTodo });
            newTodo = string.Empty;
        }
    }
}
```

チェックボックスを追加する

```
@page "/todo"
@rendermode InteractiveServer

<h3>Todo (@todos.Count(todo => !todo.IsDone))</h3>

<input @bind="newTodo" />
<button @onclick="AddTodo">Add todo</button>

<ul>
    @foreach (var todo in todos)
    {
        <li>
            <input type="checkbox" @bind="todo.IsDone" />
            <input @bind=todo.Title />
        </li>
    }
</ul>

@code {
    private List<TodoItem> todos = new();

    string newTodo = string.Empty;
    private void AddTodo()
    {
        if (!string.IsNullOrWhiteSpace(newTodo))
        {
            todos.Add(new TodoItem { Title = newTodo });
            newTodo = string.Empty;
        }
    }
}
```

![alt text](03_todo1.png)

## Blazor Web アプリでデータを操作する

### サービスの定義

データソースからUIに表示するデータを取得するサービスを定義する。

Blazorで使用できるデータソースには、RDS、NoSQL、Webサービス、Azureサービス、その他多くのシステムが含まれる。  
Entitiy Framework、HTTPクライアント、ODBCなどの.NET技術を用いてそれらのソースに対してクエリを実行できる。

まず、取得するデータを表現するためのクラスを定義する。データに関するクラスなので `Data` というメンバー名前空間を割り当てる。

```cpp
namespace BlazingPizza.Data;

public class Pizza
{
    public int PizzaId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool Vegetarian { get; set; }
    public bool Vegan { get; set; }
}
```

次に、データを取得するサービスを定義する。

```cpp
namespace BlazingPizza.Data;

public class PizzaService
{
    public Task<Pizza[]> GetPizzasAsync()
    {
      // データアクセス
    }
}
```

データソースへのアクセスには通常時間がかかる可能性があるため非同期呼び出しを使用し、データクラスのコレクションを取得する。

最後に、 `Program.cs` にコードを追加し、サービスを登録する。  

```cpp
...
// ピザサービスを追加
builder.Services.AddSingleton<PizzaService>();
...
```

これは依存性注入という仕組みで、依存先のクラスのインスタンスをインターフェースを経由して参照することで疎結合になるようにする。  
BlazorのDIでは、インスタンスの有効な範囲が決められる。

- Transient …… コンポーネントがアクセスされる度にインスタンスを生成する
- Scoped …… 利用ユーザー単位でインスタンスを生成する。コンポーネント間で共通のインスタンスが利用される
- Singleton …… アプリケーション全体でインスタンスが生成される。
  - Blazor Serverの場合、アプリケーションプロセスはサーバー側なので、ユーザー間でも共通のインスタンスが利用される
  - Blazor WebAssemblyの場合、アプリケーションプロセスはクライアント側なので、Scopedと同じ動作になる

今の例では全てのユーザーに共通する商品情報なので、Singletonスコープで登録できる。

参考：[BlazorにおけるDIのScopeについて](https://zenn.dev/yoshi1220/articles/22b99b1e3717e3)

サービスを呼び出すためにはDIでインスタンスを取得する。後ろにコンポーネント内で使用するサービスのインスタンス名をつけられる。

```cpp
@using BlazingPizza.Data
@inject PizzaService PizzaSvc
```

サービスからデータを取得する場合、 `OnInitializedAsync` メソッドで実行するのが適切らしい。  
これは、コンポーネントの初期化が完了し、初期パラメータを受け取った後、ページがレンダリングされる前に発生する。

このイベントをオーバーライドしてデータを取得する。非同期呼び出しなので `await` キーワードを使用する。

```cpp
private Pizza[] todaysPizzas;

protected override async Task OnInitializedAsync()
{
  todaysPizzas = await PizzaSvc.GetPizzaAsync();
}
```
