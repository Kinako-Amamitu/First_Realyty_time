<?php
header('X-FRAME-OPTIONS:DENY');
?>
@extends('layouts.app')
@section('title','ユーザー一覧')
@section('h1','▼ユーザー一覧▼')
@section('body')

    <table class="table table-bordered mx-auto p-2" style="width: 60%">
        {{$accounts->links()}}
        <tr>
            <th>アカウント名</th>
            <th>パスワードハッシュ</th>
            <th>変更</th>
        </tr>

        @foreach ($accounts as $account)
            <tr>
                <td>{{$account['name']}}</td>
                <td>{{$account['password']}}</td>
                <td>
                    <form method="post" action="{{route('accounts.d_check')}}">
                        @csrf
                        <button type="submit" class="btn btn-secondary">削除</button>
                        <input type="hidden" value="delete">
                        <input type="hidden" name="id" value="{{$account['id']}}">
                    </form>


                    <form method="get" action="{{route('accounts.u_check')}}">
                        @csrf
                        <button type="submit" class="btn btn-secondary">更新</button>
                        <input type="hidden" name="action" value="update">
                        <input type="hidden" name="id" value="{{$account['id']}}">

                    </form>
            </tr>
        @endforeach

    </table>
@endsection
