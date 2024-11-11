<?php

namespace App\Http\Controllers;

use App\Models\Account;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Hash;

class Dologin extends Controller
{
    public function index(Request $request)
    {
        return view('accounts/dologin');
    }

    //ログインの処理
    public function dologin(Request $request)
    {
        //ログインチェック
        $request->session()->put('login', true);
        if (!$request->session()->exists('login')) {
            //ログインにリダイレクト
            return redirect('/');
        }

        //バリデーション
        $validated = $request->validate([
            'name' => ['required', 'min:4', 'max:20'],
            'password' => ['required']
        ]);

        //条件を指定して入手
        $accounts = Account::where('name', '=', $request['name'])->get();

        if ($accounts->count() == 0) {
            return redirect()->route('login', ['error' => 'invalid']);
        } elseif (Hash::check($request['password'], $accounts[0]['password'])) {
            return redirect()->route('accounts.index');
        } else {
            return redirect()->route('login', ['error' => 'invalid']);
        }
    }

    //ログアウト処理
    public function logout(Request $request)
    {
        $request->session()->forget('login');
        $request->session()->flush();
        return redirect('/');
    }
}
