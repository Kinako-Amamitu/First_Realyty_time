<?php

namespace App\Http\Controllers;

use App\Models\User;
use Illuminate\Http\Request;

class FrendController extends Controller
{
    public function index(Request $request)
    {
        // モデル取得
        $user = User::findOrFail($request->id);

        //関連モデルからデータ取得
        if (!empty($user)) {
            $users = $user->follows()->paginate(1);
            $users->appends(['id' => $request->id]);
        }

        return view('friend.friendList', ['user' => $user ?? null]);
    }
}
