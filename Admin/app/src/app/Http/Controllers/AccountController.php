<?php

namespace App\Http\Controllers;

use App\Models\Account;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Termwind\Components\Dd;

class AccountController extends Controller
{
    //キャンセル
    public function cansel(Request $request)
    {
        return redirect()->route('accounts.index');
    }

    //アカウント登録画面
    public function create(Request $request)
    {
        return view('accounts/create');
    }

    //アカウントを追加
    public function store(Request $request)
    {
        $request->validate([
            'name' => ['required', 'unique:accounts'],
            'password' => ['min:4', 'required', 'confirmed']
        ]);
        Account::create(['name' => $request['name'], 'password' => Hash::make($request['password'])]);
        return redirect()->route('accounts.create');
    }

    //アカウント削除確認画面
    public function d_check(Request $request)
    {
        //条件を指定して入手
        $accounts = Account::where('id', '=', $request['id'])->get();
        return view('accounts.d_check', ['account' => $accounts[0]]);
    }

    //アカウント削除処理
    public function delete(Request $request)
    {
//idで検索後にレコードを削除(delete)
        $account = Account::findOrFail($request['id']);
        $account->delete();
        return redirect()->route('accounts.index');
    }

    //アカウント更新入力画面
    public function u_check(Request $request)
    {
        //条件を指定して入手
        $accounts = Account::where('id', '=', $request['id'])->get();
        return view('accounts.u_check', ['account' => $accounts[0]]);
    }

    //アカウント更新処理
    public function update(Request $request)
    {
        $request->validate([
            'password' => ['required', 'confirmed']
        ]);
        $account = Account::findOrFail($request['id']);
        $account['password'] = Hash::make($request['password']);
        $account->save();
        return redirect()->route('accounts.update_end', ['account' => $account]);
    }

    //アカウント更新完了処理
    public function update_end(Request $request)
    {
        return view('accounts.index');
    }

    //アカウント一覧表示
    public function index(Request $request)
    {
        //テーブルのすべてのレコードを取得
        $accounts = Account::ALL();
        //1ページに10件表示
        $accounts = Account::simplePaginate(10);
        return view('accounts/index', ['accounts' => $accounts]);
    }

    public function destroy(Request $request)
    {
        //ログインセッションチェック
        if (!$request->session()->exists('login')) {
            //ログインにリダイレクト
            return redirect('/');
        }
        //idで検索後にレコードを削除
        $account = Account::findOrFail(2);
        $account->delete();
    }
}
