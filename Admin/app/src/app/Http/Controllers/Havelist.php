<?php

namespace App\Http\Controllers;

use App\Models\Have;
use App\Models\User;
use Illuminate\Http\Request;

class Havelist extends Controller
{
    //アイテム一覧表示
    public function have(Request $request)
    {
        if (!$request->session()->exists('login')) {
            //ログインにリダイレクト
            return redirect('/');
        }
        //モデル取得
        $user = User::find(1);

        return view('haves/havelist', ['users' => $user]);
    }

}
