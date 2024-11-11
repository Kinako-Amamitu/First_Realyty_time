<?php

namespace App\Http\Controllers;

use App\Models\User;
use Illuminate\Http\Request;

class PlayerController extends Controller
{
    //プレイヤー一覧表示
    public function index(Request $request)
    {
        //ログインセッションチェック
        if (!$request->session()->exists('login')) {
            //ログインにリダイレクト
            return redirect('/');
        }
        //DBのすべての要素入手
        $users = User::all();
        //1ページに10件表示
        $users = User::paginate(10);
        return view('players/playerlist', ['accounts' => $users]);
    }
}
